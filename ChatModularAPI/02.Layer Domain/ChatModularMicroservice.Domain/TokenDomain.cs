using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using Utils = ChatModularMicroservice.Shared.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace ChatModularMicroservice.Domain
{
    public class TokenDomain
    {
        #region Interfaces
        private ITokenRepository _TokenRepository { get; set; }
        #endregion

        #region Constructor 
        public TokenDomain(ITokenRepository tokenRepository)
        {
            _TokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        }
        #endregion

        #region Method Publics 
        public async Task<Utils.ItemResponseDT> CreateToken(Token token)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var createdToken = await _TokenRepository.CreateTokenAsync(token);
            if (createdToken == null)
            {
                throw new Exception("Error creating token");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> EditToken(Token token)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (!await _TokenRepository.UpdateTokenAsync(token))
            {
                throw new Exception("Error updating token");
            }
            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeleteToken(string tokenId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _TokenRepository.DeleteTokenAsync(tokenId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetTokenById(TokenFilter filter, TokenFilterItemType filterType)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _TokenRepository.GetItem(filter, filterType);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> GetTokenList(TokenFilter filter, TokenFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            var tokens = await _TokenRepository.GetLstItem(filter, filterType, pagination);
            lst.LstItem = tokens.Cast<object>().ToList();
            return lst;
        }

        public async Task<Utils.ItemResponseDT> TokenExists(string tokenId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _TokenRepository.TokenExistsAsync(tokenId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> ValidateToken(string tokenValue)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _TokenRepository.ValidateTokenAsync(tokenValue);
            return item;
        }

        public async Task<Utils.ItemResponseDT> GenerateToken(string userId, string appCode)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var tokenValue = await _TokenRepository.GenerateTokenAsync(userId, appCode);
            if (string.IsNullOrEmpty(tokenValue))
            {
                throw new Exception("Error generating token");
            }
            
            item.Item = tokenValue;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> RefreshToken(string refreshToken)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var newToken = await _TokenRepository.RefreshTokenAsync(refreshToken);
            if (string.IsNullOrEmpty(newToken))
            {
                throw new Exception("Error refreshing token");
            }
            
            item.Item = newToken;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> RevokeToken(string tokenValue)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _TokenRepository.RevokeTokenAsync(tokenValue);
            return item;
        }
        #endregion
    }
}