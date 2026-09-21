using AGRIMARKET.RESOURCES.RESOURCE.ENUMS;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.HELPERS.HELPER.PAGINATION;
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


    Task<long> AddNewCommodityAsync(CommodityDto market, CancellationToken cancellation);
    Task<CommodityDto> GetCommodityByIdAsync(long Id);
     Task<PaginatedResult<CommodityDto>> GetAllCommoditiesAsync(CancellationToken cancellation, int pageSize, int pageNum);


    Task<PriceDto> GetLatestAsync(CancellationToken cancellation);
    Task<PaginatedResult<PriceDto>> GetPriceHistoryAsync(DateTime? from, DateTime? to, CancellationToken cancellation, int pageSize, int pageNum);
    //Task<CompareDto?> ComparePricesAsync(DateTime oldDate, DateTime newDate, CancellationToken cancellation);
    Task<CompareDto?> ComparePricesAsync(long commodityId, long marketId, DateTimeOffset oldDate, DateTimeOffset newDate, PriceType priceType, CancellationToken cancellation);
    Task<PaginatedResult<PriceDto>> GetAllPricesAsync(CancellationToken cancellation, int pageSize, int pageNum);
    Task<PriceDto> GetPriceByIdAsync(long Id);
    Task<long> AddNewPriceAsync(PriceDto market, CancellationToken cancellation);
    Task DeletePricesAsync(long Id, CancellationToken cancellation);

    //Profit mkt
    Task<List<ProfitDto>> FindProfitableMarketsAsync(long commodityId, long originMarketId, decimal quantity, CancellationToken cancellationToken = default);

    }
