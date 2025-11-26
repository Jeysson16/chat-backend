using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum AppRegistroFilterItemType : byte
    {
        Undefined,
        ById,
        ByAplicacionId,
        ByToken,
        ByFechaCreacion,
        ByEstadoActivo
    }

    public enum AppRegistroFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByAplicacion,
        ByActivos,
        ByInactivos,
        ByFechaCreacion
    }
}