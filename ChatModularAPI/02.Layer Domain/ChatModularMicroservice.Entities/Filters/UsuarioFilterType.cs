using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum UsuarioFilterItemType : byte
    {
        Undefined,
        ById,
        ByNombre,
        ByApellido,
        ByEmail,
        ByTelefono,
        ByFechaCreacion,
        ByEstadoActivo
    }

    public enum UsuarioFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByNombre,
        ByEmail,
        ByActivos,
        ByInactivos,
        ByFechaCreacion
    }
}