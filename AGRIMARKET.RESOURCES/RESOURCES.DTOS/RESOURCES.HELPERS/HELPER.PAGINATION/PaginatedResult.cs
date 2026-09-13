using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.RESOURCES.RESOURCES.DTOS.RESOURCES.HELPERS.HELPER.PAGINATION;

public  class PaginatedResult<T>
{
    public List<T>? Data { get; set; }
    public int pageSize { get; set; }
    public int totalCount { get; set; }
    public int pageNum { get; set; }
    public int pageCount => (int)Math.Ceiling((double)totalCount/pageSize);
}
