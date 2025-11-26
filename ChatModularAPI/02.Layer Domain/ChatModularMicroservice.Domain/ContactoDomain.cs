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
    public class ContactoDomain
    {
        #region Interfaces
        private IContactoRepository _ContactoRepository { get; set; }
        #endregion

        #region Constructor 
        public ContactoDomain(IContactoRepository contactoRepository)
        {
            _ContactoRepository = contactoRepository ?? throw new ArgumentNullException(nameof(contactoRepository));
        }
        #endregion

        #region Method Publics 
        public async Task<Utils.ItemResponseDT> CreateContacto(Contacto contacto)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            // Mapear de Domain.Contacto a Entities.Models.Contacto
            var entityContacto = new Entities.Models.Contacto
            {
                nContactosId = contacto.nContactosId,
                cUsuarioSolicitanteId = contacto.cUsuarioSolicitanteId,
                cUsuarioDestinoId = contacto.cUsuarioDestinoId,
                cEstado = contacto.cEstado,
                dFechaSolicitud = contacto.dFechaSolicitud,
                dFechaRespuesta = contacto.dFechaRespuesta,
                nEmpresaId = contacto.nEmpresaId,
                nAplicacionId = contacto.nAplicacionId
            };
            
            var createdContacto = await _ContactoRepository.CreateContactoAsync(entityContacto);
            if (createdContacto == null)
            {
                throw new Exception("Error creating contact");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> UpdateContacto(Contacto contacto)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Mapear de Domain.Contacto a Entities.Models.Contacto
            var entityContacto = new Entities.Models.Contacto
            {
                nContactosId = contacto.nContactosId,
                cUsuarioSolicitanteId = contacto.cUsuarioSolicitanteId,
                cUsuarioDestinoId = contacto.cUsuarioDestinoId,
                cEstado = contacto.cEstado,
                dFechaSolicitud = contacto.dFechaSolicitud,
                dFechaRespuesta = contacto.dFechaRespuesta,
                nEmpresaId = contacto.nEmpresaId,
                nAplicacionId = contacto.nAplicacionId
            };

            if (!await _ContactoRepository.UpdateContactoAsync(entityContacto))
            {
                throw new Exception("Error updating contact");
            }
            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeleteContacto(string contactoId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _ContactoRepository.DeleteContactoAsync(contactoId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetContactoById(ContactoFilter filter, ContactoFilterItemType filterType)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _ContactoRepository.GetItem(filter, filterType);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> GetContactoList(ContactoFilter filter, ContactoFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            var contactos = await _ContactoRepository.GetLstItem(filter, filterType, pagination);
            lst.LstItem = contactos?.Cast<object>().ToList() ?? new List<object>();
            return lst;
        }

        public async Task<Utils.ItemResponseDT> ContactoExists(string contactoId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _ContactoRepository.ContactoExistsAsync(contactoId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> AddContacto(Contacto contacto)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Mapear de Domain.Contacto a Entities.Models.Contacto
            var entityContacto = new Entities.Models.Contacto
            {
                nContactosId = contacto.nContactosId,
                cUsuarioSolicitanteId = contacto.cUsuarioSolicitanteId,
                cUsuarioDestinoId = contacto.cUsuarioDestinoId,
                cEstado = contacto.cEstado,
                dFechaSolicitud = contacto.dFechaSolicitud,
                dFechaRespuesta = contacto.dFechaRespuesta,
                nEmpresaId = contacto.nEmpresaId,
                nAplicacionId = contacto.nAplicacionId
            };

            if (!await _ContactoRepository.AddContactoAsync(entityContacto))
            {
                throw new Exception("Error adding contact");
            }

            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> RemoveContacto(string contactoId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (!await _ContactoRepository.RemoveContactoAsync(contactoId))
            {
                throw new Exception("Error removing contact");
            }

            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDTLst> SearchUsuarios(string searchTerm, string usuarioId, string empresaId, string aplicacionId)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            var usuarios = await _ContactoRepository.BuscarUsuariosAsync(searchTerm, usuarioId, empresaId, aplicacionId, "1");
            lst.LstItem = usuarios?.Cast<object>().ToList() ?? new List<object>();
            return lst;
        }
        #endregion
    }
}
