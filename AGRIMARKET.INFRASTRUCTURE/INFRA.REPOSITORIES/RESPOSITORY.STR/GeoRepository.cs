using AGRIMARKET.APPLICATION.APPLICATION.IR.IR.STR;
using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.MKT;
using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.STR;
using AGRIMARKET.INFRASTRUCTURE.INFRA.CONTEXT;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.STR;
using AGRIMARKET.RESOURCES.RESOURCES.HELPERS.HELPER.PAGINATION;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.RESPOSITORY.STR;

public  class GeoRepository : IGeoRepoistory
{
    private readonly AgriMarketContext agriMarketContext;
    private readonly ILogger<GeoRepository> logger;

    public GeoRepository(AgriMarketContext agri,ILogger<GeoRepository> _logger)
    {
        agriMarketContext = agri;
        logger = _logger;

    }
    #region[Regions]
    public async Task<PaginatedResult<RegionDto>> GetAllRegionsAsync(CancellationToken cancellation, int pageSize, int pageNum)
    {
        if (pageNum < 1) pageNum = 1;
        if (pageSize < 1) pageSize = 1;
        var entity = agriMarketContext.Regions.AsNoTracking();
        var totalCount = await entity.CountAsync(cancellation);
        var records = await entity.OrderByDescending(x => x.Id)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new RegionDto
            {
                Name = x.Name,
                Districts = x.Districts.Select(ds => new DistictDto
                {
                    Name = ds.Name,

                }).ToList(),
            }).ToListAsync(cancellation);
        return new PaginatedResult<RegionDto>
        {
            pageNum = pageNum,
            pageSize = pageSize,
            totalCount = totalCount,
            Data = records
        };

    }
    public async Task<RegionDto> GetRegionByIdAsync(long Id)
    {
        if (Id < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(Id));
        }
        var data = await agriMarketContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
        return new RegionDto
        {
            Name = data?.Name,
            Districts = data.Districts.Select(x => new DistictDto
            {
                Name = x.Name,
            }).ToList()
        };
    }
    public async Task<long> AddNewRegionsAsync(RegionDto region, CancellationToken cancellation)
    {
        if (string.IsNullOrWhiteSpace(region.Name))
        {
            throw new ArgumentNullException(nameof(region));
        }
        var entityData = new RegionModel
        {
            Name = region.Name,
            Districts = region.Districts.Select(ds => new DistrictModel
            {
                Name = ds.Name,

            }).ToList(),
        };
        await agriMarketContext.Regions.AddRangeAsync(entityData);
        await agriMarketContext.SaveChangesAsync(cancellation);
        return region.Id;
    }

    public async Task DeleteRegionAsync(long Id, CancellationToken cancellation)
    {
        if (Id < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(Id));
        }
        var deleteData = await agriMarketContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
        if (deleteData != null)
            agriMarketContext.Regions.RemoveRange(deleteData);
        await agriMarketContext.SaveChangesAsync(cancellation);
    }
    #endregion

    #region[DISTRICTS]
    public async Task<PaginatedResult<DistictDto>> GetAllDistrictsAsync(CancellationToken cancellation, int pageSize, int pageNum)
    {
        if (pageNum < 1) pageNum = 1;
        if (pageSize < 1) pageSize = 1;
        var entity = agriMarketContext.Districts.AsNoTracking();
        var totalCount = await entity.CountAsync(cancellation);
        var records = await entity.OrderByDescending(x => x.Id)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new DistictDto
            {
                Name = x.Name,
                Markets = x.Markets.Select(mk => new MarketDto
                {
                    Name = mk.Name
                }).ToList(),
            }).ToListAsync(cancellation);
        return new PaginatedResult<DistictDto>
        {
            pageNum = pageNum,
            pageSize = pageSize,
            totalCount = totalCount,
            Data = records
        };

    }
    public async Task<DistictDto> GetDistrictByIdAsync(long Id)
    {
        if (Id < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(Id));
        }
        var data = await agriMarketContext.Districts.FirstOrDefaultAsync(x => x.Id == Id);
        return new DistictDto
        {
            Name = data?.Name,
            Markets = data.Markets.Select(x => new MarketDto
            {
                Name = x.Name,
            }).ToList()
        };
    }
    public async Task<long> AddNewDistrcictsAsync(DistictDto distcrict, CancellationToken cancellation)
    {
        if (string.IsNullOrWhiteSpace(distcrict.Name))
        {
            throw new ArgumentNullException(nameof(distcrict));
        }
        var entityData = new DistrictModel
        {
            Name = distcrict.Name,
            Markets = distcrict.Markets.Select(ds => new MarketModel
            {
                Name = ds.Name,

            }).ToList(),
        };
        await agriMarketContext.Districts.AddRangeAsync(entityData);
        await agriMarketContext.SaveChangesAsync(cancellation);
        return distcrict.Id;
    }

    public async Task DeleteDsistrictAsync(long Id, CancellationToken cancellation)
    {
        if (Id < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(Id));
        }
        var deleteData = await agriMarketContext.Districts.FirstOrDefaultAsync(x => x.Id == Id);
        if (deleteData != null)
            agriMarketContext.Districts.RemoveRange(deleteData);
        await agriMarketContext.SaveChangesAsync(cancellation);
    }
    #endregion


}
