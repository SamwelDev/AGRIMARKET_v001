using AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;
using AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICES.IS;
using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.MKT;
using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.STR;
using AGRIMARKET.INFRASTRUCTURE.INFRA.CONTEXT;
using AGRIMARKET.RESOURCES.RESOURCE.ENUMS;
using AGRIMARKET.RESOURCES.RESOURCES.EXT.EX.NUKTA;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.AUTO;

public class ExternalDataImporterService : IExternalDataImpoter
{
    private readonly INuktaConfigurationClieant _nuktaClient;
    private readonly AgriMarketContext _context;

    public ExternalDataImporterService(INuktaConfigurationClieant nuktaClient, AgriMarketContext context)
    {
        _nuktaClient = nuktaClient;
        _context = context;
    }

    public async Task<int> ImportPricesAsync(DateTime? date = null, CancellationToken cancellationToken = default)
    {
        var prices = await _nuktaClient
            .GetCommodityPricesAsync(
                date,
                cancellationToken);

        if (prices == null || prices.Count == 0)
            return 0;

        var source = await GetOrCreateSourceAsync(cancellationToken);
        var imported = 0;
        foreach (var item in prices)
        {
            if (string.IsNullOrWhiteSpace(item.Name) || string.IsNullOrWhiteSpace(item.Location))
            {
                continue;
            }

            var commodity = await GetOrCreateCommodityAsync(item,cancellationToken);
            var market = await GetOrCreateMarketAsync(item,cancellationToken);
            var recordedAt = item.Date == default ? new DateTimeOffset(date ?? DateTime.UtcNow): new DateTimeOffset(item.Date);
            var exists = await _context.Prices.AnyAsync(x =>
                        x.CommodityId == commodity.Id &&
                        x.MarketId == market.Id &&
                        x.SourceId == source.Id &&
                        x.RecordedAt.Date == recordedAt.Date,cancellationToken);

            if (exists)
                continue;

            var price = new PriceModel
            {
                Price = item.Price,
                MinPrice = item.MaxPrice,
                MaxPrice = item.MaxPrice,
                Curreny = CurrenyEnum.TZS,
                PriceType = PriceType.GENERAL,
                RecordedAt = recordedAt,
                SourceId = source.Id,
                CommodityId = commodity.Id,
                MarketId = market?.Id
            };

            _context.Prices.Add(price);
            imported++;
        }
        await _context.SaveChangesAsync(cancellationToken);
        return imported;
    }
    private async Task<PriceSource> GetOrCreateSourceAsync(CancellationToken cancellationToken)
    {
        var source = await _context.Sources.FirstOrDefaultAsync(x => x.Name == "Nukta AI",cancellationToken);

        if (source != null)
            return source;

        source = new PriceSource
        {
            Name = "Nukta AI"
        };
        _context.Sources.Add(source);
        await _context.SaveChangesAsync(cancellationToken);
        return source;
    }
    private async Task<MarketModel?> GetOrCreateMarketAsync(NuktaCommodityPriceDto item,CancellationToken cancellationToken)
    {
        var location = item.Location!.Trim();
        var market = await _context.Markets
            .Include(x => x.District)
            .FirstOrDefaultAsync(
                x => x.Name == location,
                cancellationToken);

        if (market != null)
            return market;

        var district =await _context.Districts.FirstOrDefaultAsync(x => x.Name == location,cancellationToken);
        if (district == null)
        {
            district = new DistrictModel
            {
                Name = location,
            };

            _context.Districts.Add(district);
            await _context.SaveChangesAsync(cancellationToken);
        }
        market = new MarketModel
        {
            Name = location,
            DistrictId = district.Id
        };
        _context.Markets.Add(market);
        await _context.SaveChangesAsync(cancellationToken);
        return market;
    }
    private async Task<CommodityModel> GetOrCreateCommodityAsync(NuktaCommodityPriceDto item,CancellationToken cancellationToken)
    {
        var name = item.Name!.Trim();
        var commodity =await _context.Commodities.FirstOrDefaultAsync(x => x.Name == name,cancellationToken);
        if (commodity != null)
            return commodity;

        commodity = new CommodityModel
        {
            Name = name,
            Unit = UnitEnum.KG
        };
        _context.Commodities.Add(commodity);
        await _context.SaveChangesAsync(cancellationToken);
        return commodity;
    }
    
}
