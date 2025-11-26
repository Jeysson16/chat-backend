using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum ContactoFilterItemType : byte
    {
        Undefined,
        ById,
        ByNombre,
        ByApellido,
        ByEmail,
        ByTelefono,
        ByEmpresaId,
        ByEstadoActivo,
        // Add missing item type used in repository
        ByEstado,
        ByUsuarios,
        ByUsuarioSolicitante,
        ByEmpresaYAplicacion
    }

    public enum ContactoFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByEmpresa,
        ByNombre,
        ByEmail,
        ByActivos,
        ByInactivos,
        // Add missing list types used in repository
        ByEstado,
        ByTerminoBusqueda,
        ByUsuarioSolicitante,
        ByEmpresaYAplicacion
    }
}