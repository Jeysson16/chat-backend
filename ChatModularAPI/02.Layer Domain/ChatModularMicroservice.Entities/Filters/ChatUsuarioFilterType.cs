using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum ChatUsuarioFilterItemType : byte
    {
        Undefined,
        ById,
        ByUsuarioId,
        ByConversacionId,
        ByRol,
        ByFechaUnion,
        ByEstadoActivo
    }

    public enum ChatUsuarioFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByUsuario,
        ByConversacion,
        ByRol,
        ByActivos,
        ByInactivos,
        ByFechaUnion
    }
}