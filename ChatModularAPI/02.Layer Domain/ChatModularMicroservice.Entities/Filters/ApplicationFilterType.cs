using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum ApplicationFilterItemType : byte
    {
        Undefined,
        ById,
        ByNombre,
        ByCodigo,
        ByDescripcion,
        ByEstadoActivo
    }

    public enum ApplicationFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByActivas,
        ByInactivas,
        // Add missing list types used in repository
        ByVersion,
        ByTerminoBusqueda
    }
}