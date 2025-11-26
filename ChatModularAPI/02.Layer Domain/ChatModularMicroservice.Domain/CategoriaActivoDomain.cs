using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using Utils = ChatModularMicroservice.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ChatModularMicroservice.Domain
{
    public class CategoriaActivoDomain
    {
        #region Interfaces
        private ICategoriaActivoRepository _CategoriaActivoRepository { get; set; }
        #endregion

        #region Constructor 
        public CategoriaActivoDomain(ICategoriaActivoRepository categoriaActivo)
        {
            _CategoriaActivoRepository = categoriaActivo ?? throw new ArgumentNullException(nameof(categoriaActivo));
        }
        #endregion

        #region Method Publics 
        public async Task<Utils.ItemResponseDT> CreateCategoriaActivo(CategoriaActivoEntity categoriaActivo)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            long id = await _CategoriaActivoRepository.Insert(categoriaActivo);
            if (id == 0)
            {
                throw new Exception("Error inserting categoria activo");
            }
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> EditCategoriaActivo(CategoriaActivoEntity categoriaActivo)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (!await _CategoriaActivoRepository.Update(categoriaActivo))
            {
                throw new Exception("Error updating categoria activo");
            }
            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeleteCategoriaActivo(Int32 nCategoriaActivoCodigo)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _CategoriaActivoRepository.DeleteEntero(nCategoriaActivoCodigo);
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetCategoriaActivoById(CategoriaActivoFilter filter, CategoriaActivoFilterItemType filterType)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _CategoriaActivoRepository.GetItem(filter, filterType);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> GetCategoriaActivoList(CategoriaActivoFilter filter, CategoriaActivoFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            lst.LstItem = (await _CategoriaActivoRepository.GetLstItem(filter, filterType, pagination)).Cast<object>().ToList();
            return lst;
        }
        #endregion
    }
}
