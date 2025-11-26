using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum ConfiguracionEmpresaFilterItemType : byte
    {
        Undefined,
        ById,
        ByEmpresaId,
        ByTipoConfiguracionId,
        ByValor,
        ByEstadoActivo
    }

    public enum ConfiguracionEmpresaFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByEmpresa,
        ByTipoConfiguracion,
        ByActivas,
        ByInactivas
    }
}