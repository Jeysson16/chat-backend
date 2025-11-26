using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum TokenRegistroFilterItemType : byte
    {
        Undefined,
        ById,
        ByToken,
        ByUsuarioId,
        ByTipo,
        ByFechaCreacion,
        ByFechaExpiracion,
        ByEstadoActivo,
        // Add missing item types used in repository
        ByJwtToken,
        ByPerJurYPerCodigo,
        ByAppCodigo
    }

    public enum TokenRegistroFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByUsuario,
        ByTipo,
        ByActivos,
        ByInactivos,
        ByExpirados,
        ByFechaCreacion,
        // Add missing list types used in repository
        ByUsuarioId,
        ByPerJurYPerCodigo,
        ByTerminoBusqueda,
        ByAppCodigo,
        All
    }
}