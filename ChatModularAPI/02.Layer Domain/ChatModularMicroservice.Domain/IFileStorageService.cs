using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Domain
{
    public interface IFileStorageService
    {
        Task<MessageAttachmentDto> UploadAsync(Stream stream, string fileName, string contentType, string appCode, string? subfolder, CancellationToken cancellationToken = default);
        Task<string> GetPublicUrlAsync(string path, string appCode);
        Task<string> CreateSignedUrlAsync(string path, string appCode, int expiresInSeconds = 604800);
    }
}

