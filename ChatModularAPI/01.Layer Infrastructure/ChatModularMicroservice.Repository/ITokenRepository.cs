using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository
{
    public interface ITokenRepository : IDeleteIntRepository, IInsertIntRepository<TokenRegistro>, IUpdateRepository<TokenRegistro>
    {
        Task<TokenRegistro> GetItem(TokenFilter filter, TokenFilterItemType filterType);
        Task<IEnumerable<TokenRegistro>> GetLstItem(TokenFilter filter, TokenFilterListType filterType, Utils.Pagination pagination);
        
        // Métodos adicionales para gestión de tokens
        Task<bool> TokenExistsAsync(string tokenId);
        Task<bool> ValidateTokenAsync(string tokenValue);
        Task<string> GenerateTokenAsync(string userId, string appCode);
        Task<string> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string tokenValue);
        Task<bool> IsTokenActiveAsync(string jwtToken);
        Task<TokenRegistro?> GetRefreshTokenAsync(string refreshToken);
        Task<bool> UpdateRefreshTokenAsync(long tokenCodigo, string newRefreshToken, string? usuarioId = null);
        
        // Métodos CRUD adicionales
        Task<TokenRegistro?> CreateTokenAsync(TokenRegistro token);
        Task<bool> UpdateTokenAsync(TokenRegistro token);
        Task<bool> DeleteTokenAsync(string tokenId);
    }
}