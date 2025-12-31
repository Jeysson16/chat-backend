using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Shared.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Supabase;
using System.Globalization;
using System.Text;

namespace ChatModularMicroservice.Infrastructure
{
    public class SupabaseStorageService : IFileStorageService
    {
        private readonly Supabase.Client _client;
        private readonly ILogger<SupabaseStorageService> _logger;
        private readonly string _bucket;
        private bool _bucketEnsured;

        public SupabaseStorageService(Supabase.Client client, IConfiguration configuration, ILogger<SupabaseStorageService> logger)
        {
            _client = client;
            _logger = logger;
            _bucket = configuration["Supabase:AttachmentsBucket"] ?? "chat-attachments";
        }

        public async Task<MessageAttachmentDto> UploadAsync(Stream stream, string fileName, string contentType, string appCode, string? subfolder, CancellationToken cancellationToken = default)
        {
            var safeApp = SanitizeSegment(string.IsNullOrWhiteSpace(appCode) ? "default" : appCode.Trim().ToLowerInvariant());
            var folder = SanitizeSegment(string.IsNullOrWhiteSpace(subfolder) ? "general" : subfolder.Trim());
            var guid = Guid.NewGuid().ToString("N");
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var safeName = SanitizeFileName(fileName);
            var path = $"{safeApp}/{folder}/{date}/{guid}_{safeName}";

            var fileOptions = new Supabase.Storage.FileOptions { ContentType = contentType };

            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms, cancellationToken);
            var bytes = ms.ToArray();

            await EnsureBucketAsync();

            try
            {
                await _client.Storage.From(_bucket).Upload(bytes, path, fileOptions, null);
            }
            catch (Supabase.Storage.Exceptions.SupabaseStorageException ex) when (ex.Message?.IndexOf("Bucket not found", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                _logger.LogWarning(ex, "Bucket '{Bucket}' no existe, intentando crearlo y reintentando subida", _bucket);
                await EnsureBucketCreatedAsync();
                await _client.Storage.From(_bucket).Upload(bytes, path, fileOptions, null);
            }

            var url = _client.Storage.From(_bucket).GetPublicUrl(path);

            return new MessageAttachmentDto
            {
                id = guid,
                name = safeName,
                url = url,
                type = GetAttachmentTypeFromMime(contentType),
                size = stream.CanSeek ? stream.Length : 0,
                mimeType = contentType,
                path = path
            };
        }

        public Task<string> GetPublicUrlAsync(string path, string appCode)
        {
            _ = EnsureBucketAsync();
            var url = _client.Storage.From(_bucket).GetPublicUrl(path);
            return Task.FromResult(url);
        }

        public async Task<string> CreateSignedUrlAsync(string path, string appCode, int expiresInSeconds = 604800)
        {
            await EnsureBucketAsync();
            var signed = await _client.Storage.From(_bucket).CreateSignedUrl(path, expiresInSeconds);
            return signed;
        }

        private static string GetAttachmentTypeFromMime(string mime)
        {
            if (string.IsNullOrEmpty(mime)) return "file";
            if (mime.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) return "image";
            if (mime.StartsWith("audio/", StringComparison.OrdinalIgnoreCase)) return "audio";
            return "file";
        }

        private async Task EnsureBucketAsync()
        {
            if (_bucketEnsured) return;
            try
            {
                await EnsureBucketCreatedAsync();
                _bucketEnsured = true;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "No se pudo asegurar bucket '{Bucket}' en inicialización", _bucket);
            }
        }

        private async Task EnsureBucketCreatedAsync()
        {
            try
            {
                await _client.Storage.CreateBucket(_bucket);
                _logger.LogInformation("Bucket '{Bucket}' creado", _bucket);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Omitiendo error al crear bucket '{Bucket}' (puede que ya exista)", _bucket);
            }
        }

        private static string SanitizeSegment(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "general";
            var normalized = RemoveDiacritics(input.ToLowerInvariant());
            var sb = new StringBuilder(normalized.Length);
            foreach (var ch in normalized)
            {
                if ((ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9') || ch == '-' || ch == '_')
                    sb.Append(ch);
            }
            var s = sb.Length == 0 ? "general" : sb.ToString();
            return s.Length > 64 ? s.Substring(0, 64) : s;
        }

        private static string SanitizeFileName(string fileName)
        {
            var name = Path.GetFileName(fileName ?? "file");
            var ext = Path.GetExtension(name);
            var baseName = string.IsNullOrEmpty(ext) ? name : name[..^ext.Length];
            baseName = RemoveDiacritics(baseName.ToLowerInvariant());
            var sb = new StringBuilder(baseName.Length);
            foreach (var ch in baseName)
            {
                if ((ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9') || ch == '-' || ch == '_' )
                    sb.Append(ch);
                else if (char.IsWhiteSpace(ch))
                    sb.Append('-');
            }
            var safeBase = sb.Length == 0 ? "file" : sb.ToString();
            if (safeBase.Length > 80) safeBase = safeBase.Substring(0, 80);
            var safeExt = string.IsNullOrEmpty(ext) ? string.Empty : ext.ToLowerInvariant().Replace(".", string.Empty);
            if (safeExt.Length > 16) safeExt = safeExt.Substring(0, 16);
            return string.IsNullOrEmpty(safeExt) ? safeBase : $"{safeBase}.{safeExt}";
        }

        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalized.Length);
            foreach (var ch in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
