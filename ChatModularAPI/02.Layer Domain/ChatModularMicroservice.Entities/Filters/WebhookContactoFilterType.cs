using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum WebhookContactoFilterItemType : byte
    {
        Undefined,
        ById,
        ByWebhookId,
        ByContactoId,
        ByTipoEvento,
        ByEstadoActivo,
        ByFechaCreacion
    }

    public enum WebhookContactoFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByWebhook,
        ByContacto,
        ByTipoEvento,
        ByActivos,
        ByInactivos,
        ByFechaCreacion
    }
}