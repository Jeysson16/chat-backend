using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum AdquisicionFilterItemType : byte
    {
        Undefined,
        ById
    }

    public enum AdquisicionFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree
    }
}
