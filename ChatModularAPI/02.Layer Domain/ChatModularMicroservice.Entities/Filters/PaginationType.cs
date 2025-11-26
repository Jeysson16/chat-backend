using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities
{
    public enum PaginationFilterItemType : byte
    {
        Undefined,
        ByPageNumber,
        ByPageSize,
        BySkip
    }

    public enum PaginationFilterListType : byte
    {
        Undefined,
        ByPagination,
        ByTree,
        ByDefault,
        ByCustomSize
    }
}