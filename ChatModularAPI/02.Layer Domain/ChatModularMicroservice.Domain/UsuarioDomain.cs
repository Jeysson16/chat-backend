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
    public class UsuarioDomain
    {
        #region Interfaces
        private IUsuarioRepository _UsuarioRepository { get; set; }
        #endregion

        #region Constructor 
        public UsuarioDomain(IUsuarioRepository usuarioRepository)
        {
            _UsuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
        }
        #endregion

        #region Method Publics 
        public async Task<Utils.ItemResponseDT> AddUsuario(Usuario usuario)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            // Mapear de Domain.Usuario a Entities.Models.Usuario
            var entityUsuario = new ChatModularMicroservice.Entities.Models.Usuario
            {
                cNombre = usuario.cNombre,
                cApellido = usuario.cApellido,
                cEmail = usuario.cEmail,
                cTelefono = usuario.cTelefono,
                cAvatar = usuario.cAvatar,
                bActivo = usuario.bActivo,
                nEmpresaId = usuario.nEmpresaId,
                nAplicacionId = usuario.nAplicacionId
            };
            
            var createdUsuario = await _UsuarioRepository.CreateAsync(entityUsuario);
            if (createdUsuario == null)
            {
                throw new Exception("Error creating usuario");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> EditUsuario(Usuario usuario)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Mapear de Domain.Usuario a Entities.Models.Usuario
            var entityUsuario = new ChatModularMicroservice.Entities.Models.Usuario
            {
                cNombre = usuario.cNombre,
                cApellido = usuario.cApellido,
                cEmail = usuario.cEmail,
                cTelefono = usuario.cTelefono,
                cAvatar = usuario.cAvatar,
                bActivo = usuario.bActivo,
                nEmpresaId = usuario.nEmpresaId,
                nAplicacionId = usuario.nAplicacionId
            };

            var updatedUsuario = await _UsuarioRepository.UpdateAsync(entityUsuario);
            if (updatedUsuario == null)
            {
                throw new Exception("Error updating usuario");
            }
            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeleteUsuario(string usuarioId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            if (Guid.TryParse(usuarioId, out Guid id))
            {
                item.Item = await _UsuarioRepository.DeleteAsync(id);
            }
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetUsuarioById(UsuarioFilter filter, UsuarioFilterItemType filterType)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _UsuarioRepository.GetItem(filter, filterType);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> GetUsuarioList(UsuarioFilter filter, UsuarioFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            var result = await _UsuarioRepository.GetLstItem(filter, filterType, pagination);
            lst.LstItem = result.Cast<object>().ToList();
            return lst;
        }

        public async Task<Utils.ItemResponseDT> UsuarioExists(string usuarioId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            if (Guid.TryParse(usuarioId, out Guid id))
            {
                item.Item = await _UsuarioRepository.ExistsAsync(id);
            }
            return item;
        }

        public async Task<Utils.ItemResponseDT> ValidateUsuario(string email, string password)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            var usuario = await _UsuarioRepository.GetByEmailAsync(email);
            if (usuario != null)
            {
                // Aquí deberías implementar la validación de contraseña según tu lógica de negocio
                // Por ahora, simplemente verificamos que el usuario existe
                item.Item = true;
            }
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetUsuarioByEmail(string email)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _UsuarioRepository.GetByEmailAsync(email);
            return item;
        }

        public async Task<Utils.ItemResponseDT> UpdatePassword(string usuarioId, string newPassword)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            if (!await _UsuarioRepository.UpdatePasswordAsync(usuarioId, newPassword))
            {
                throw new Exception("Error updating password");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> ActivateUsuario(string usuarioId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            if (!await _UsuarioRepository.ActivateUsuarioAsync(usuarioId))
            {
                throw new Exception("Error activating usuario");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeactivateUsuario(string usuarioId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            if (!await _UsuarioRepository.DeactivateUsuarioAsync(usuarioId))
            {
                throw new Exception("Error deactivating usuario");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> ResetPassword(string email)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var resetToken = await _UsuarioRepository.GeneratePasswordResetTokenAsync(email);
            if (string.IsNullOrEmpty(resetToken))
            {
                throw new Exception("Error generating password reset token");
            }
            
            item.Item = resetToken;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> ConfirmPasswordReset(string token, string newPassword)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            if (!await _UsuarioRepository.ConfirmPasswordResetAsync(token, newPassword))
            {
                throw new Exception("Error confirming password reset");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }
        #endregion
    }
}