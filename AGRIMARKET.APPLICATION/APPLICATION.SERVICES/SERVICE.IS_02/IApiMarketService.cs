using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.STR;
using AGRIMARKET.RESOURCES.RESOURCES.HELPERS.HELPER.PAGINATION;
using AGRIMARKET.RESOURCES.RESOURCES.RESPONSE;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICE.IS_02;

public interface IApiMarketService
{
    Task<List<ProfitDto>> FindProfitableMarketsAsync(long commodityId, long originMarketId, decimal quantity, CancellationToken cancellationToken = default);
    Task<PaginatedResult<PriceDto>?> GetPricesAsync(int pageNum = 1, int pageSize = 100, CancellationToken cancellationToken = default);
    Task<PaginatedResult<RegionDto>?> GetRegionsAsync(int pageNum = 1, int pageSize = 50, CancellationToken cancellationToken = default);
}
