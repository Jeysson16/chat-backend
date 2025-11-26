using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum EmpresaFilterItemType : byte
    {
        Undefined,
        ById,
        ByNombre,
        ByCodigo,
        ByAplicacionId,
        ByFechaCreacion,
        ByEstadoActivo
    }

    public enum EmpresaFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByAplicacion,
        ByActivas,
        ByInactivas,
        ByFechaCreacion,
        // Add missing list type used in repository
        ByTerminoBusqueda
    }
}