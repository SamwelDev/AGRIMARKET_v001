using AGRIMARKET.APPLICATION.APPLICATION.IR.IR.MKT;
using AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;
using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.MKT;
using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.STR;
using AGRIMARKET.INFRASTRUCTURE.INFRA.CONTEXT;
using AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.RESPOSITORY.STR;
using AGRIMARKET.INFRASTRUCTURE.INFRA.SV;
using AGRIMARKET.RESOURCES.RESOURCE.ENUMS;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.STR;
using AGRIMARKET.RESOURCES.RESOURCES.HELPERS.HELPER.PAGINATION;
using AGRIMARKET.RESOURCES.RESOURCES.RESPONSE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.REPOSITORY.MKT;

public class MktRepository : IMktRepository
{
    private readonly AgriMarketContext agriMarketContext;
    private readonly ILogger<MktRepository> logger;
    private readonly IRouteConfigurationClient client;

    public MktRepository(AgriMarketContext agriMarket, ILogger<MktRepository> _logger,IRouteConfigurationClient _client)
    {
        agriMarketContext = agriMarket;
        logger = _logger;
        client = _client;
    }
    #region[MKT]
    public async Task<PaginatedResult<MarketDto>> GetAllMarketsAsync(CancellationToken cancellation, int pageSize, int pageNum)
    {
        if (pageNum < 1) pageNum = 1;
        if (pageSize < 1) pageSize = 1;
        var entity = agriMarketContext.Markets.AsNoTracking();
        var totalCount = await entity.CountAsync(cancellation);
        var records = await entity.OrderByDescending(x => x.Id)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new MarketDto
            {
                Name = x.Name,
                Prices = x.Prices.Select(ds => new PriceDto
                {
                    MaxPrice = ds.MaxPrice,
                    MinPrice = ds.MinPrice,
                    Price = ds.Price,
                    PriceType = ds.PriceType,
                    Curreny = ds.Curreny,
                }).ToList(),
            }).ToListAsync(cancellation);
        return new PaginatedResult<MarketDto>
        {
            pageNum = pageNum,
            pageSize = pageSize,
            totalCount = totalCount,
            Data = records
        };

    }
    public async Task<MarketDto> GetMarketByIdAsync(long Id)
    {
        if (Id < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(Id));
        }
        var data = await agriMarketContext.Markets.FirstOrDefaultAsync(x => x.Id == Id);
        return new MarketDto
        {
            Name = data?.Name,
            Prices = data.Prices.Select(x => new PriceDto
            {
                PriceType = x.PriceType,
                Curreny = x.Curreny,
            }).ToList()
        };
    }
    public async Task<long> AddNewMarketAsync(MarketDto market, CancellationToken cancellation)
    {
        if (string.IsNullOrWhiteSpace(market.Name))
        {
            throw new ArgumentNullException(nameof(market));
        }
        var entityData = new MarketModel
        {
            Name = market.Name,
            Prices = market.Prices.Select(ds => new PriceModel
            {
                Curreny = ds.Curreny,
                PriceType =ds.PriceType,
                MaxPrice = ds.MaxPrice,
                MinPrice = ds.MinPrice,
                Price = ds.Price,
            }).ToList(),
        };
        await agriMarketContext.Markets.AddRangeAsync(entityData);
        await agriMarketContext.SaveChangesAsync(cancellation);
        return market.Id;
    }

    public async Task DeleteRegionAsync(long Id, CancellationToken cancellation)
    {
        if (Id < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(Id));
        }
        var deleteData = await agriMarketContext.Markets.FirstOrDefaultAsync(x => x.Id == Id);
        if (deleteData != null)
            agriMarketContext.Markets.RemoveRange(deleteData);
        await agriMarketContext.SaveChangesAsync(cancellation);
    }
    public async Task<List<ProfitDto>> FindProfitableMarketsAsync(long commodityId,long originMarketId,decimal quantity,CancellationToken cancellationToken = default)
    {
       
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.",nameof(quantity));
        }
        var commodityExists = await agriMarketContext.Commodities
            .AsNoTracking()
            .AnyAsync(
                x => x.Id == commodityId,
                cancellationToken);

        if (!commodityExists)
        {
            throw new InvalidOperationException($"Commodity with ID {commodityId} was not found.");
        }
        var originMarket = await agriMarketContext.Markets.AsNoTracking().FirstOrDefaultAsync(x => x.Id == originMarketId,cancellationToken);
        if (originMarket == null)
        {
            throw new InvalidOperationException(
                $"Origin market with ID {originMarketId} was not found.");
        }
        if (!originMarket.Latitude.HasValue || !originMarket.Longitude.HasValue)
        {
            throw new InvalidOperationException($"Origin market '{originMarket.Name}' does not have coordinates.");
        }
        var prices = await agriMarketContext.Prices.AsNoTracking().Include(x => x.Market)
            .Where(x =>x.CommodityId == commodityId && x.MarketId.HasValue && x.Market != null && x.Market.Latitude.HasValue &&
                x.Market.Longitude.HasValue)
            .ToListAsync(cancellationToken);
        if (prices.Count == 0)
        {
            return [];
        }
        var latestPrices = prices.GroupBy(x => x.MarketId!.Value).Select(group => group.OrderByDescending(x => x.RecordedAt).First()).ToList();
        var destinations = latestPrices
            .Where(x => x.MarketId.HasValue && x.MarketId.Value != originMarketId && x.Market != null &&x.Market.Latitude.HasValue &&x.Market.Longitude.HasValue)
            .Select(x => new RouteDestinationRequest
            {
                MarketId = x.MarketId!.Value,
                Latitude = x.Market!.Latitude!.Value,
                Longitude = x.Market.Longitude!.Value
            })
            .ToList();
        if (destinations.Count == 0)
        {
            return [];
        }
        var distances = await client.GetDistancesAsync(originMarket.Latitude.Value,originMarket.Longitude.Value,destinations,cancellationToken);
        if (distances.Count == 0)
        {
            return [];
        }
        var results = new List<ProfitDto>();
        foreach (var price in latestPrices)
        {
            if (!price.MarketId.HasValue)
            {
                continue;
            }
            if (price.MarketId.Value == originMarketId)
            {
                continue;
            }
            var distance = distances.FirstOrDefault(
                x => x.MarketId == price.MarketId.Value);

            if (distance == null)
            {
                continue;
            }
            var grossRevenue = price.Price * quantity;
            results.Add(new ProfitDto
            {
                MarketId = price.MarketId.Value,
                MarketName = price.Market?.Name,
                PricePerUnit = price.Price,
                Quantity = quantity,
                DistanceKm = distance.DistanceKm,
                DurationMinutes = distance.DurationMinutes,
                GrossRevenue = grossRevenue,
                EstimatedTransportCost = 0,
                EstimatedNetRevenue = grossRevenue,
                Currency = "TZS"
            });
        }
        return results.OrderByDescending(x => x.GrossRevenue).ToList();
    }   
    #endregion

    #region[CMDTIES]
    public async Task<PaginatedResult<CommodityDto>> GetAllCommoditiesAsync(CancellationToken cancellation, int pageSize, int pageNum)
    {
        if (pageNum < 1) pageNum = 1;
        if (pageSize < 1) pageSize = 1;
        var entity = agriMarketContext.Commodities.AsNoTracking();
        var totalCount = await entity.CountAsync(cancellation);
        var records = await entity.OrderByDescending(x => x.Id)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new CommodityDto
            {
                Name = x.Name,
                Prices = x.Prices.Select(ds => new PriceDto
                {
                    MaxPrice = ds.MaxPrice,
                    MinPrice = ds.MinPrice,
                    Price = ds.Price,
                    PriceType = ds.PriceType,
                    Curreny = ds.Curreny,
                }).ToList(),
            }).ToListAsync(cancellation);
        return new PaginatedResult<CommodityDto>
        {
            pageNum = pageNum,
            pageSize = pageSize,
            totalCount = totalCount,
            Data = records
        };

    }
    public async Task<CommodityDto> GetCommodityByIdAsync(long Id)
    {
        if (Id < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(Id));
        }
        var data = await agriMarketContext.Markets.FirstOrDefaultAsync(x => x.Id == Id);
        return new CommodityDto
        {
            Name = data?.Name,

            Prices = data.Prices.Select(x => new PriceDto
            {
                PriceType = x.PriceType,
                Curreny = x.Curreny,
            }).ToList()
        };
    }
    public async Task<long> AddNewCommodityAsync(CommodityDto market, CancellationToken cancellation)
    {
        if (string.IsNullOrWhiteSpace(market.Name))
        {
            throw new ArgumentNullException(nameof(market));
        }
        var entityData = new CommodityModel
        {
            Name = market.Name,
            Prices = market.Prices.Select(ds => new PriceModel
            {
                Curreny = ds.Curreny,
                PriceType = ds.PriceType,
                MaxPrice = ds.MaxPrice,
                MinPrice = ds.MinPrice,
                Price = ds.Price,
            }).ToList(),
        };
        await agriMarketContext.Commodities.AddRangeAsync(entityData);
        await agriMarketContext.SaveChangesAsync(cancellation);
        return market.Id;
    }
    #endregion

    #region[PRICES]
 
        public  async Task<PriceDto> GetLatestAsync(CancellationToken cancellation)
        {
            var data = await agriMarketContext.Prices.AsNoTracking().OrderByDescending(x => x.RecordedAt)
                .FirstOrDefaultAsync(cancellation);
            if (data == null)
                return null;
            return new PriceDto
            {
                Curreny = data.Curreny,
                PriceType = data.PriceType,
                MaxPrice = data.MaxPrice,
                MinPrice = data.MinPrice,
                Price = data.Price,
                RecordedAt = data.RecordedAt
            };
        }
        public async Task<PaginatedResult<PriceDto>> GetPriceHistoryAsync(DateTime? from,DateTime? to,CancellationToken cancellation,int pageSize,int pageNum)
        {
            if (pageNum < 1)
                pageNum = 1;

            if (pageSize < 1)
                pageSize = 20;

            var query = agriMarketContext.Prices
                .AsNoTracking()
                .AsQueryable();

            if (from.HasValue)
            {
                query = query.Where(x => x.RecordedAt >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(x => x.RecordedAt <= to.Value);
            }

            var totalCount = await query.CountAsync(cancellation);

            var records = await query
                .OrderByDescending(x => x.RecordedAt)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PriceDto
                {
                    Id = x.Id,
                    MaxPrice = x.MaxPrice,
                    MinPrice = x.MinPrice,
                    Curreny = x.Curreny,
                    PriceType = x.PriceType,
                    RecordedAt = x.RecordedAt
                })
                .ToListAsync(cancellation);

            return new PaginatedResult<PriceDto>
            {
                pageNum = pageNum,
                pageSize = pageSize,
                totalCount = totalCount,
                Data = records
            };
        }

        public async Task<CompareDto?> ComparePricesAsync(long commodityId,long marketId,DateTimeOffset oldDate,DateTimeOffset newDate,PriceType priceType,CancellationToken cancellation)
        {
            var oldPrice = await agriMarketContext.Prices
                .AsNoTracking()
                .Where(x =>
                    x.CommodityId == commodityId &&
                    x.MarketId == marketId &&
                    x.PriceType == priceType &&
                    x.RecordedAt <= oldDate)
                .OrderByDescending(x => x.RecordedAt)
                .FirstOrDefaultAsync(cancellation);

            var newPrice = await agriMarketContext.Prices
                .AsNoTracking()
                .Where(x =>
                    x.CommodityId == commodityId &&
                    x.MarketId == marketId &&
                    x.PriceType == priceType &&
                    x.RecordedAt <= newDate)
                .OrderByDescending(x => x.RecordedAt)
                .FirstOrDefaultAsync(cancellation);

            if (oldPrice == null || newPrice == null)
                return null;

            return new CompareDto
            {
                PriceType = newPrice.PriceType.ToString(),
                OldPrice = oldPrice.Price,
                NewPrice = newPrice.Price,
                PriceDifference = newPrice.Price - oldPrice.Price,
                PricePercentageChange =CalculatePercentageChange(oldPrice.Price,newPrice.Price),
                OldMinPrice = oldPrice.MinPrice,
                NewMinPrice = newPrice.MinPrice,
                MinPriceDifference =newPrice.MinPrice - oldPrice.MinPrice,
                MinPricePercentageChange =CalculatePercentageChange(oldPrice.MinPrice,newPrice.MinPrice),
                OldMaxPrice = oldPrice.MaxPrice,
                NewMaxPrice = newPrice.MaxPrice,
                MaxPriceDifference =newPrice.MaxPrice - oldPrice.MaxPrice,
                MaxPricePercentageChange = CalculatePercentageChange(oldPrice.MaxPrice,newPrice.MaxPrice),
                OldRecordedAt = oldPrice.RecordedAt,
                NewRecordedAt = newPrice.RecordedAt
            };
        }
        public async Task<PaginatedResult<PriceDto>> GetAllPricesAsync(CancellationToken cancellation,int pageNum,int pageSize)
        {
            if (pageNum < 1)pageNum = 1;
            if (pageSize < 1)pageSize = 1;
            var entity = agriMarketContext.Prices.AsNoTracking();
            var totalCount = await entity.CountAsync(cancellation);
            var records = await entity
                .OrderByDescending(x => x.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PriceDto
                {
                    Id = x.Id,

                    MaxPrice = x.MaxPrice,
                    MinPrice = x.MinPrice,
                    Region = x.Market != null && x.Market.District != null &&x.Market.District.Region != null ? x.Market.District.Region.Name: null,
                    Price = x.Price,
                    Curreny = x.Curreny,
                    PriceType = x.PriceType,
                    RecordedAt = x.RecordedAt,
                    SourceId = x.SourceId,
                    CommodityId = x.CommodityId,
                    MarketId = x.MarketId,
                    Commodity = x.Commodity != null? x.Commodity.Name: null,
                    Unit = x.Commodity != null? x.Commodity.Unit.ToString(): null,
                    Market = x.Market != null? x.Market.Name: null,
                    District = x.Market != null &&x.Market.District != null? x.Market.District.Name: null
                })
                .ToListAsync(cancellation);

            return new PaginatedResult<PriceDto>
            {
                pageNum = pageNum,
                pageSize = pageSize,
                totalCount = totalCount,
                Data = records
            };
        }
        public async Task<PriceDto> GetPriceByIdAsync(long Id)
        {
            if (Id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(Id));
            }
            var data = await agriMarketContext.Prices.FirstOrDefaultAsync(x => x.Id == Id);
            return new PriceDto
            {
                MaxPrice = data.MaxPrice,
                Price = data.Price,
                MinPrice = data.MinPrice,
                Curreny = data.Curreny,
                PriceType = data.PriceType,
            };
        }
        public async Task<long> AddNewPriceAsync(PriceDto market, CancellationToken cancellation)
        {
            if (market.MaxPrice>0)
            {
                throw new ArgumentNullException(nameof(market));
            }
            if (market.MaxPrice > 0)
            {
                throw new ArgumentNullException(nameof(market));
            }
            var entityData = new PriceModel
            {
                Curreny = market.Curreny,
                PriceType = market.PriceType,
                RecordedAt = market.RecordedAt,
                MaxPrice = market.MaxPrice,
                MinPrice = market.MinPrice,
                Price = market.Price,
            
            };
            await agriMarketContext.Prices.AddRangeAsync(entityData);
            await agriMarketContext.SaveChangesAsync(cancellation);
            return market.Id;
        }

        public async Task DeletePricesAsync(long Id, CancellationToken cancellation)
        {
            if (Id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(Id));
            }
            var deleteData = await agriMarketContext.Prices.FirstOrDefaultAsync(x => x.Id == Id);
            if (deleteData != null)
                agriMarketContext.Prices.RemoveRange(deleteData);
            await agriMarketContext.SaveChangesAsync(cancellation);
        }
    #endregion

    #region[Others]
    private static decimal? CalculatePercentageChange(decimal oldValue,decimal newValue)
    {
        if (oldValue == 0)
            return null;

        return ((newValue - oldValue) / oldValue) * 100;
    }
    #endregion

}
