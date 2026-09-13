using AGRIMARKET.APPLICATION.APPLICATION.IR.IR.MKT;
using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.MKT;
using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.STR;
using AGRIMARKET.INFRASTRUCTURE.INFRA.CONTEXT;
using AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.RESPOSITORY.STR;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.STR;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.RESOURCES.HELPERS.HELPER.PAGINATION;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.REPOSITORY.MKT;

public class MktRepository : IMktRepository
{
    private readonly AgriMarketContext agriMarketContext;
    private readonly ILogger<MktRepository> logger;

    public MktRepository(AgriMarketContext agriMarket, ILogger<MktRepository> _logger)
    {
        agriMarketContext = agriMarket;
        logger = _logger;
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
    #endregion

}
