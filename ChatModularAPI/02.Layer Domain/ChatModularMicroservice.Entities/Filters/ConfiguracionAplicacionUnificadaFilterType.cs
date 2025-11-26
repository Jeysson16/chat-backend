using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum ConfiguracionAplicacionUnificadaFilterItemType : byte
    {
        Undefined,
        ById,
        ByAplicacionId,
        ByClave,
        ByValor,
        ByTipo,
        ByEstadoActivo
    }

    public enum ConfiguracionAplicacionUnificadaFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByAplicacion,
        ByTipo,
        ByActivas,
        ByInactivas
    }
}