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
    public class ConfiguracionEmpresaDomain
    {
        #region Interfaces
        private IConfiguracionEmpresaRepository _ConfiguracionEmpresaRepository { get; set; }
        #endregion

        #region Constructor 
        public ConfiguracionEmpresaDomain(IConfiguracionEmpresaRepository configuracionEmpresaRepository)
        {
            _ConfiguracionEmpresaRepository = configuracionEmpresaRepository ?? throw new ArgumentNullException(nameof(configuracionEmpresaRepository));
        }
        #endregion

        #region Method Publics 
        public async Task<Utils.ItemResponseDT> CreateConfiguracionEmpresa(ConfiguracionEmpresa configuracion)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            // Convertir de Domain a Entities.Models
            var entityConfig = new Entities.Models.ConfiguracionEmpresa
            {
                nConfigEmpresaId = configuracion.nConfigEmpresaId,
                cConfigEmpresaNombre = configuracion.cConfigEmpresaNombre,
                cConfigEmpresaDescripcion = configuracion.cConfigEmpresaDescripcion,
                cConfigEmpresaDominio = configuracion.cConfigEmpresaDominio,
                cConfigEmpresaColorPrimario = configuracion.cConfigEmpresaColorPrimario,
                cConfigEmpresaColorSecundario = configuracion.cConfigEmpresaColorSecundario,
                cConfigEmpresaUrlLogo = configuracion.cConfigEmpresaUrlLogo,
                cConfigEmpresaFuentePersonalizada = configuracion.cConfigEmpresaFuentePersonalizada,
                nConfigEmpresaMaxUsuarios = configuracion.nConfigEmpresaMaxUsuarios,
                nConfigEmpresaMaxCanales = configuracion.nConfigEmpresaMaxCanales,
                nConfigEmpresaCuotaAlmacenamientoGB = configuracion.nConfigEmpresaCuotaAlmacenamientoGB,
                nConfigEmpresaTiempoSesionMinutos = configuracion.nConfigEmpresaTiempoSesionMinutos,
                bConfigEmpresaHabilitarCompartirArchivos = configuracion.bConfigEmpresaHabilitarCompartirArchivos,
                bConfigEmpresaHabilitarNotificaciones = configuracion.bConfigEmpresaHabilitarNotificaciones,
                bConfigEmpresaHabilitarIntegraciones = configuracion.bConfigEmpresaHabilitarIntegraciones,
                bConfigEmpresaHabilitarAnaliticas = configuracion.bConfigEmpresaHabilitarAnaliticas,
                bConfigEmpresaEsActiva = configuracion.bConfigEmpresaEsActiva,
                dConfigEmpresaFechaCreacion = configuracion.dConfigEmpresaFechaCreacion,
                dConfigEmpresaFechaActualizacion = configuracion.dConfigEmpresaFechaActualizacion,
                nConfigEmpresaAplicacionId = configuracion.nConfigEmpresaAplicacionId,
                nConfigEmpresaEmpresaId = configuracion.nConfigEmpresaEmpresaId
            };
            
            var createdConfig = await _ConfiguracionEmpresaRepository.CreateConfiguracionEmpresaAsync(entityConfig);
            if (createdConfig == null)
            {
                throw new Exception("Error creating empresa configuration");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> EditConfiguracionEmpresa(ConfiguracionEmpresa configuracion)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Convertir de Domain a Entities.Models
            var entityConfig = new Entities.Models.ConfiguracionEmpresa
            {
                nConfigEmpresaId = configuracion.nConfigEmpresaId,
                cConfigEmpresaNombre = configuracion.cConfigEmpresaNombre,
                cConfigEmpresaDescripcion = configuracion.cConfigEmpresaDescripcion,
                cConfigEmpresaDominio = configuracion.cConfigEmpresaDominio,
                cConfigEmpresaColorPrimario = configuracion.cConfigEmpresaColorPrimario,
                cConfigEmpresaColorSecundario = configuracion.cConfigEmpresaColorSecundario,
                cConfigEmpresaUrlLogo = configuracion.cConfigEmpresaUrlLogo,
                cConfigEmpresaFuentePersonalizada = configuracion.cConfigEmpresaFuentePersonalizada,
                nConfigEmpresaMaxUsuarios = configuracion.nConfigEmpresaMaxUsuarios,
                nConfigEmpresaMaxCanales = configuracion.nConfigEmpresaMaxCanales,
                nConfigEmpresaCuotaAlmacenamientoGB = configuracion.nConfigEmpresaCuotaAlmacenamientoGB,
                nConfigEmpresaTiempoSesionMinutos = configuracion.nConfigEmpresaTiempoSesionMinutos,
                bConfigEmpresaHabilitarCompartirArchivos = configuracion.bConfigEmpresaHabilitarCompartirArchivos,
                bConfigEmpresaHabilitarNotificaciones = configuracion.bConfigEmpresaHabilitarNotificaciones,
                bConfigEmpresaHabilitarIntegraciones = configuracion.bConfigEmpresaHabilitarIntegraciones,
                bConfigEmpresaHabilitarAnaliticas = configuracion.bConfigEmpresaHabilitarAnaliticas,
                bConfigEmpresaEsActiva = configuracion.bConfigEmpresaEsActiva,
                dConfigEmpresaFechaCreacion = configuracion.dConfigEmpresaFechaCreacion,
                dConfigEmpresaFechaActualizacion = configuracion.dConfigEmpresaFechaActualizacion,
                nConfigEmpresaAplicacionId = configuracion.nConfigEmpresaAplicacionId,
                nConfigEmpresaEmpresaId = configuracion.nConfigEmpresaEmpresaId
            };

            if (!await _ConfiguracionEmpresaRepository.UpdateConfiguracionEmpresaAsync(entityConfig))
            {
                throw new Exception("Error updating empresa configuration");
            }
            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeleteConfiguracionEmpresa(string configuracionId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _ConfiguracionEmpresaRepository.DeleteConfiguracionEmpresaAsync(configuracionId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetConfiguracionEmpresaById(ConfiguracionEmpresaFilter filter, ConfiguracionEmpresaFilterItemType filterType)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _ConfiguracionEmpresaRepository.GetItem(filter, filterType);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> GetConfiguracionEmpresaList(ConfiguracionEmpresaFilter filter, ConfiguracionEmpresaFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            var result = await _ConfiguracionEmpresaRepository.GetLstItem(filter, filterType, pagination);
            lst.LstItem = result?.Cast<object>().ToList();
            return lst;
        }

        public async Task<Utils.ItemResponseDT> ConfiguracionEmpresaExists(string configuracionId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _ConfiguracionEmpresaRepository.ConfiguracionEmpresaExistsAsync(configuracionId);
            return item;
        }
        #endregion
    }
}
