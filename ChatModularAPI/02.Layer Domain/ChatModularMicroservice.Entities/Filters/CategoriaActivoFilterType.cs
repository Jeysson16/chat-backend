using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum CategoriaActivoFilterItemType : byte
    {
        Undefined,
        ById,
        ByNombre,
        ByCodigo,
        ByDescripcion,
        ByEstadoActivo,
        ByFechaEfectiva,
        ByUsuarioRegistro
    }

    public enum CategoriaActivoFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByActivas,
        ByInactivas,
        ByNombre,
        ByFechaRegistro
    }
}