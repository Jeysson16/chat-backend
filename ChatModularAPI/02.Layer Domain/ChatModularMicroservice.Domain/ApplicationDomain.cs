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
    public class ApplicationDomain
    {
        #region Interfaces
        private IApplicationRepository _ApplicationRepository { get; set; }
        #endregion

        #region Constructor 
        public ApplicationDomain(IApplicationRepository applicationRepository)
        {
            _ApplicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
        }
        #endregion

        #region Method Publics 
        public async Task<Utils.ItemResponseDT> CreateApplication(Application application)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            // Convertir de Domain a Entities.Models
            var entityApp = new Entities.Models.Application
            {
                nAplicacionesId = application.nAplicacionesId,
                cAplicacionesNombre = application.cAplicacionesNombre,
                cAplicacionesCodigo = application.cAplicacionesCodigo,
                cAplicacionesDescripcion = application.cAplicacionesDescripcion,
                cAplicacionesUrlLogo = application.cAplicacionesUrlLogo,
                bAplicacionesEsActiva = application.bAplicacionesEsActiva,
                dAplicacionesFechaCreacion = application.dAplicacionesFechaCreacion,
                dAplicacionesFechaActualizacion = application.dAplicacionesFechaActualizacion
            };
            
            var createdApp = await _ApplicationRepository.CreateApplicationAsync(entityApp);
            if (createdApp == null)
            {
                throw new Exception("Error creating application");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> EditApplication(Application application)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var entityApplication = new Entities.Models.Application
            {
                nAplicacionesId = application.nAplicacionesId,
                cAplicacionesNombre = application.cAplicacionesNombre,
                cAplicacionesDescripcion = application.cAplicacionesDescripcion,
                cAplicacionesCodigo = application.cAplicacionesCodigo,
                bAplicacionesEsActiva = application.bAplicacionesEsActiva,
                dAplicacionesFechaCreacion = application.dAplicacionesFechaCreacion
            };

            var updateResult = await _ApplicationRepository.UpdateApplicationAsync(entityApplication);
            if (updateResult == null)
            {
                throw new Exception("Error updating application");
            }
            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeleteApplication(string applicationId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            if (int.TryParse(applicationId, out int appId))
            {
                item.Item = await _ApplicationRepository.DeleteApplicationAsync(appId);
            }
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetApplicationById(ApplicationFilter filter, ApplicationFilterItemType filterType)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _ApplicationRepository.GetItem(filter, filterType);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> GetApplicationList(ApplicationFilter filter, ApplicationFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            lst.LstItem = (await _ApplicationRepository.GetLstItem(filter, filterType, pagination)).Cast<object>().ToList();
            return lst;
        }

        public async Task<Utils.ItemResponseDT> ApplicationExists(string applicationId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            if (int.TryParse(applicationId, out int id))
            {
                item.Item = await _ApplicationRepository.ApplicationExistsAsync(id);
            }
            return item;
        }
        #endregion
    }
}
