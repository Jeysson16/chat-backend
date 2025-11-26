using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum ChatFilterItemType : byte
    {
        Undefined,
        ById,
        ByAppCodigo,
        ByNombre,
        ByDescripcion,
        ByTipo,
        ByEstadoActivo,
        ByFechaCreacion,
        // Add missing item type used in repository
        ByAppCodigoYUsuario
    }

    public enum ChatFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByTipo,
        ByAppCodigo,
        ByUsuarioCreador,
        ByActivas,
        ByInactivas,
        ByFechaCreacion,
        // Add missing list types used in repository
        ByTerminoBusqueda,
        All
    }
}