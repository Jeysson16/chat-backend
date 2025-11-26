using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Repository;
using ChatModularMicroservice.Infrastructure.Repositories;
using ChatModularMicroservice.Shared.Configs;
using Microsoft.Extensions.Logging;
using Supabase;

namespace ChatModularMicroservice.Infrastructure
{
    public class TokenSupabaseRepository : SupabaseRepository, ITokenRepository
    {
        public TokenSupabaseRepository(Client supabaseClient, ILogger<TokenSupabaseRepository> logger, SupabaseConfig config)
            : base(supabaseClient, logger, config) { }

        public async Task<bool> TokenExistsAsync(string tokenId)
        {
            var res = await _supabaseClient
                .From<TokenRegistro>()
                .Where(t => t.cTokenRegistroJwtToken == tokenId)
                .Get();
            return (res.Models?.Any() ?? false);
        }

        public async Task<bool> ValidateTokenAsync(string tokenValue)
        {
            return await TokenExistsAsync(tokenValue);
        }

        public Task<string> GenerateTokenAsync(string userId, string appCode)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }

        public Task<string> RefreshTokenAsync(string refreshToken)
        {
            return Task.FromResult(refreshToken);
        }

        public Task<bool> RevokeTokenAsync(string tokenValue)
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsTokenActiveAsync(string jwtToken)
        {
            return Task.FromResult(true);
        }

        public Task<TokenRegistro?> GetRefreshTokenAsync(string refreshToken)
        {
            return Task.FromResult<TokenRegistro?>(null);
        }

        public Task<bool> UpdateRefreshTokenAsync(long tokenCodigo, string newRefreshToken, string? usuarioId = null)
        {
            return Task.FromResult(true);
        }

        public Task<TokenRegistro?> CreateTokenAsync(TokenRegistro token)
        {
            return Task.FromResult<TokenRegistro?>(token);
        }

        public Task<bool> UpdateTokenAsync(TokenRegistro token)
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeleteTokenAsync(string tokenId)
        {
            return Task.FromResult(true);
        }

        public Task<int> Insert(TokenRegistro item)
        {
            return Task.FromResult(0);
        }

        public Task<bool> Update(TokenRegistro item)
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeleteEntero(int id)
        {
            return Task.FromResult(true);
        }
        public Task<TokenRegistro> GetItem(ChatModularMicroservice.Entities.TokenFilter filter, ChatModularMicroservice.Entities.TokenFilterItemType filterType)
        {
            throw new NotSupportedException();
        }

        public Task<IEnumerable<TokenRegistro>> GetLstItem(ChatModularMicroservice.Entities.TokenFilter filter, ChatModularMicroservice.Entities.TokenFilterListType filterType, ChatModularMicroservice.Shared.Utils.Pagination pagination)
        {
            throw new NotSupportedException();
        }
    }
}