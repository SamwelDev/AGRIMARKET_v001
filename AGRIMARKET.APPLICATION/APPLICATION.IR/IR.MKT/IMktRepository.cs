using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.RESOURCES.HELPERS.HELPER.PAGINATION;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.APPLICATION.APPLICATION.IR.IR.MKT;

public  interface IMktRepository
{
    Task<PaginatedResult<MarketDto>> GetAllMarketsAsync(CancellationToken cancellation, int pageSize, int pageNum);
    Task<MarketDto> GetMarketByIdAsync(long Id);
    Task<long> AddNewMarketAsync(MarketDto market, CancellationToken cancellation);
    Task DeleteRegionAsync(long Id, CancellationToken cancellation);
}
