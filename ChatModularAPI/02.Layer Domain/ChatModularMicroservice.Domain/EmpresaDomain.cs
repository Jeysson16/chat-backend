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
    public class EmpresaDomain
    {
        #region Interfaces
        private IEmpresaRepository _EmpresaRepository { get; set; }
        #endregion

        #region Constructor 
        public EmpresaDomain(IEmpresaRepository empresaRepository)
        {
            _EmpresaRepository = empresaRepository ?? throw new ArgumentNullException(nameof(empresaRepository));
        }
        #endregion

        #region Method Publics 
        public async Task<Utils.ItemResponseDT> CreateEmpresa(Empresa empresa)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var createDto = new CreateEmpresaDto
            {
                cEmpresasNombre = empresa.cEmpresasNombre,
                cEmpresasCodigo = empresa.cEmpresasCodigo,
                bEmpresasActiva = empresa.bEmpresasEsActiva,
                nEmpresasAplicacionId = empresa.nEmpresasAplicacionId,
                cEmpresasDominio = empresa.cEmpresasCodigo // Usando Code como dominio por defecto
            };
            
            var createdEmpresa = await _EmpresaRepository.CreateEmpresaAsync(createDto);
            if (createdEmpresa == null)
            {
                throw new Exception("Error creating empresa");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> EditEmpresa(Empresa empresa)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var updateDto = new UpdateEmpresaDto
            {
                cEmpresasNombre = empresa.cEmpresasNombre,
                cEmpresasCodigo = empresa.cEmpresasCodigo,
                bEmpresasActiva = empresa.bEmpresasEsActiva
            };

            var updatedEmpresa = await _EmpresaRepository.UpdateEmpresaAsync(empresa.nEmpresasId, updateDto);
            if (updatedEmpresa == null)
            {
                throw new Exception("Error updating empresa");
            }
            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeleteEmpresa(string empresaId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            if (int.TryParse(empresaId, out int id))
            {
                item.Item = await _EmpresaRepository.DeleteEmpresaAsync(id);
            }
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetEmpresaById(EmpresaFilter filter, EmpresaFilterItemType filterType)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _EmpresaRepository.GetItem(filter, filterType);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> GetEmpresaList(EmpresaFilter filter, EmpresaFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            var empresas = await _EmpresaRepository.GetLstItem(filter, filterType, pagination);
            lst.LstItem = empresas.Cast<object>().ToList();
            return lst;
        }

        public async Task<Utils.ItemResponseDT> EmpresaExists(string empresaId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            if (int.TryParse(empresaId, out int id))
            {
                item.Item = await _EmpresaRepository.ExistsEmpresaByIdAsync(id);
            }
            return item;
        }

        public async Task<Utils.ItemResponseDT> ValidateEmpresaCode(string empresaCode)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _EmpresaRepository.ValidateEmpresaCodeAsync(empresaCode);
            return item;
        }

        public async Task<Utils.ItemResponseDTLst> GetEmpresasByApplication(string applicationId)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            lst.LstItem = await _EmpresaRepository.GetEmpresasByApplicationAsync(applicationId);
            return lst;
        }
        #endregion
    }
}
