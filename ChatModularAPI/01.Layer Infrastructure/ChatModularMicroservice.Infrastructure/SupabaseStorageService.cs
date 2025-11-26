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

namespace ChatModularMicroservice.Infrastructure
{
    public class SupabaseStorageService : IFileStorageService
    {
        private readonly Supabase.Client _client;
        private readonly ILogger<SupabaseStorageService> _logger;
        private readonly string _bucket;

        public SupabaseStorageService(Supabase.Client client, IConfiguration configuration, ILogger<SupabaseStorageService> logger)
        {
            _client = client;
            _logger = logger;
            _bucket = configuration["Supabase:AttachmentsBucket"] ?? "chat-attachments";
        }

        public async Task<MessageAttachmentDto> UploadAsync(Stream stream, string fileName, string contentType, string appCode, string? subfolder, CancellationToken cancellationToken = default)
        {
            var safeApp = string.IsNullOrWhiteSpace(appCode) ? "default" : appCode.Trim().ToLowerInvariant();
            var folder = string.IsNullOrWhiteSpace(subfolder) ? "general" : subfolder.Trim();
            var guid = Guid.NewGuid().ToString("N");
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var path = $"{safeApp}/{folder}/{date}/{guid}_{fileName}";

            var fileOptions = new Supabase.Storage.FileOptions { ContentType = contentType };

            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms, cancellationToken);
            var bytes = ms.ToArray();

            await _client.Storage.From(_bucket).Upload(bytes, path, fileOptions, null);

            var url = _client.Storage.From(_bucket).GetPublicUrl(path);

            return new MessageAttachmentDto
            {
                id = guid,
                name = fileName,
                url = url,
                type = GetAttachmentTypeFromMime(contentType),
                size = stream.CanSeek ? stream.Length : 0,
                mimeType = contentType,
                path = path
            };
        }

        public Task<string> GetPublicUrlAsync(string path, string appCode)
        {
            var url = _client.Storage.From(_bucket).GetPublicUrl(path);
            return Task.FromResult(url);
        }

        public async Task<string> CreateSignedUrlAsync(string path, string appCode, int expiresInSeconds = 604800)
        {
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
    }
}
