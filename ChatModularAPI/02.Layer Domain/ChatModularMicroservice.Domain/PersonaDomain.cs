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
    public class PersonaDomain
    {
        #region Interfaces
        private IPersonaRepository _PersonaRepository { get; set; }
        #endregion

        #region Constructor 
        public PersonaDomain(IPersonaRepository personaRepository)
        {
            _PersonaRepository = personaRepository ?? throw new ArgumentNullException(nameof(personaRepository));
        }
        #endregion

        #region Method Publics 
        public async Task<Utils.ItemResponseDT> CreatePersona(Usuario persona)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var entityPersona = new Entities.Models.Usuario
            {
                cUsuariosId = persona.cUsuariosId,
                cNombre = persona.cNombre,
                cApellido = persona.cApellido,
                cEmail = persona.cEmail,
                cTelefono = persona.cTelefono,
                bActivo = persona.bActivo,
                dFechaCreacion = persona.dFechaCreacion,
                nEmpresaId = persona.nEmpresaId,
                nAplicacionId = persona.nAplicacionId
            };
            
            var createdPersona = await _PersonaRepository.CreateAsync(entityPersona);
            if (createdPersona == null)
            {
                throw new Exception("Error creating persona");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> EditPersona(Usuario persona)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var entityPersona = new Entities.Models.Usuario
            {
                cUsuariosId = persona.cUsuariosId,
                cNombre = persona.cNombre,
                cApellido = persona.cApellido,
                cEmail = persona.cEmail,
                cTelefono = persona.cTelefono,
                bActivo = persona.bActivo,
                dFechaCreacion = persona.dFechaCreacion,
                nEmpresaId = persona.nEmpresaId,
                nAplicacionId = persona.nAplicacionId
            };

            var updatedPersona = await _PersonaRepository.UpdateAsync(entityPersona);
            if (updatedPersona == null)
            {
                throw new Exception("Error updating persona");
            }
            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeletePersona(string personaId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _PersonaRepository.DeleteAsync(Guid.Parse(personaId));
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetPersonaById(PersonaFilter filter, PersonaFilterItemType filterType)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _PersonaRepository.GetItem(filter, filterType);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> GetPersonaList(PersonaFilter filter, PersonaFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            var personas = await _PersonaRepository.GetLstItem(filter, filterType, pagination);
            lst.LstItem = personas.Cast<object>().ToList();
            return lst;
        }

        public async Task<Utils.ItemResponseDT> PersonaExists(string personaId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _PersonaRepository.ExistsAsync(Guid.Parse(personaId));
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetPersonaByCodigo(string personaCodigo)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _PersonaRepository.GetByCodigoAsync(personaCodigo);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> SearchPersonas(string searchTerm)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            var personas = await _PersonaRepository.SearchPersonasAsync(searchTerm);
            lst.LstItem = personas?.Cast<object>().ToList() ?? new List<object>();
            return lst;
        }

        public async Task<Utils.ItemResponseDT> ValidatePersonaCode(string personaCode)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _PersonaRepository.ExistsByCodigoAsync(personaCode);
            return item;
        }
        #endregion
    }
}
