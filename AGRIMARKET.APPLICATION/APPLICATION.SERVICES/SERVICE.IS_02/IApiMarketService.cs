using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.HELPERS.HELPER.PAGINATION;
using AGRIMARKET.RESOURCES.RESOURCES.RESPONSE;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICE.IS_02;

public interface IApiMarketService
{
    Task<PaginatedResult<PriceDto>?> GetPricesAsync(int pageNum = 1, int pageSize = 100, CancellationToken cancellationToken = default);
}
