using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum WebhookFilterItemType : byte
    {
        Undefined,
        ById,
        ByUrl,
        ByAppCodigo,
        ByTipo,
        ByEstadoActivo,
        ByFechaCreacion
    }

    public enum WebhookFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByApp,
        ByTipo,
        ByActivos,
        ByInactivos,
        ByFechaCreacion
    }
}