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
    public class ConfiguracionDomain
    {
        #region Interfaces
        private IConfiguracionRepository _ConfiguracionRepository { get; set; }
        #endregion

        #region Constructor 
        public ConfiguracionDomain(IConfiguracionRepository configuracionRepository)
        {
            _ConfiguracionRepository = configuracionRepository ?? throw new ArgumentNullException(nameof(configuracionRepository));
        }
        #endregion

        #region Method Publics 
        public async Task<Utils.ItemResponseDT> CreateConfiguracion(ConfiguracionAplicacion configuracion)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            // Convertir de Domain a Entities.Models
            var entityConfig = new Entities.Models.ConfiguracionAplicacion
            {
                nAplicacionesId = configuracion.nAplicacionesId,
                nMaxTamanoArchivo = configuracion.nMaxTamanoArchivo,
                cTiposArchivosPermitidos = configuracion.cTiposArchivosPermitidos,
                bPermitirAdjuntos = configuracion.bPermitirAdjuntos,
                nMaxCantidadAdjuntos = configuracion.nMaxCantidadAdjuntos,
                bPermitirVisualizacionAdjuntos = configuracion.bPermitirVisualizacionAdjuntos,
                nMaxLongitudMensaje = configuracion.nMaxLongitudMensaje,
                bPermitirEmojis = configuracion.bPermitirEmojis,
                bPermitirMensajesVoz = configuracion.bPermitirMensajesVoz,
                bPermitirNotificaciones = configuracion.bPermitirNotificaciones
            };
            
            var createdConfig = await _ConfiguracionRepository.Insert(entityConfig);
            if (createdConfig <= 0)
            {
                throw new Exception("Error creating configuration");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> EditConfiguracion(ConfiguracionAplicacion configuracion)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Convertir de Domain a Entities.Models
            var entityConfig = new Entities.Models.ConfiguracionAplicacion
            {
                nConfiguracionAplicacionId = configuracion.nConfiguracionAplicacionId,
                nAplicacionesId = configuracion.nAplicacionesId,
                nMaxTamanoArchivo = configuracion.nMaxTamanoArchivo,
                cTiposArchivosPermitidos = configuracion.cTiposArchivosPermitidos,
                bPermitirAdjuntos = configuracion.bPermitirAdjuntos,
                nMaxCantidadAdjuntos = configuracion.nMaxCantidadAdjuntos,
                bPermitirVisualizacionAdjuntos = configuracion.bPermitirVisualizacionAdjuntos,
                nMaxLongitudMensaje = configuracion.nMaxLongitudMensaje,
                bPermitirEmojis = configuracion.bPermitirEmojis,
                bPermitirMensajesVoz = configuracion.bPermitirMensajesVoz,
                bPermitirNotificaciones = configuracion.bPermitirNotificaciones
            };

            if (!await _ConfiguracionRepository.Update(entityConfig))
            {
                throw new Exception("Error updating configuration");
            }
            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeleteConfiguracion(int configuracionId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _ConfiguracionRepository.DeleteEntero(configuracionId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetConfiguracionById(ConfiguracionFilter filter, ConfiguracionFilterItemType filterType)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _ConfiguracionRepository.GetItem(filter, filterType);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> GetConfiguracionList(ConfiguracionFilter filter, ConfiguracionFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            var result = await _ConfiguracionRepository.GetLstItem(filter, filterType, pagination);
            lst.LstItem = result?.Cast<object>().ToList();
            return lst;
        }

        public async Task<Utils.ItemResponseDT> ConfiguracionExists(int configuracionId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            var filter = new ConfiguracionFilter { ConfiguracionId = configuracionId };
            var result = await _ConfiguracionRepository.GetItem(filter, ConfiguracionFilterItemType.ConfiguracionId);
            item.Item = result != null;
            return item;
        }
        #endregion
    }
}
