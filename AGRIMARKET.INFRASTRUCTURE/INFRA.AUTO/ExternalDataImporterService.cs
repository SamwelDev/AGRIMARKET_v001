using AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;
using AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICES.IS;
using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.MKT;
using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.STR;
using AGRIMARKET.INFRASTRUCTURE.INFRA.CONTEXT;
using AGRIMARKET.RESOURCES.RESOURCE.ENUMS;
using AGRIMARKET.RESOURCES.RESOURCES.EXT.EX.NUKTA;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.AUTO;

public class ExternalDataImporterService : IExternalDataImpoter
{
    private readonly INuktaConfigurationClieant _nuktaClient;
    private readonly AgriMarketContext _context;
    private readonly ILogger<ExternalDataImporterService> _logger;

    public ExternalDataImporterService(
        INuktaConfigurationClieant nuktaClient,
        AgriMarketContext context,
        ILogger<ExternalDataImporterService> logger)
    {
        _nuktaClient = nuktaClient;
        _context = context;
        _logger = logger;
    }

    public async Task<int> ImportPricesAsync(DateTime? date = null, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Nukta price import. Date: {Date}", date?.ToString("yyyy-MM-dd") ?? "latest");
        var prices = await _nuktaClient.GetCommodityPricesAsync(date,cancellationToken);
        if (prices == null || prices.Count == 0)
        {
            _logger.LogInformation("Nukta returned no price data.");
            return 0;
        }

        _logger.LogInformation("Nukta returned {Count} price records.",prices.Count);
        var source = await GetOrCreateSourceAsync(cancellationToken);
        int imported = 0;
        int updated = 0;
        int skipped = 0;
        foreach (var item in prices)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(item.Name))
            {
                _logger.LogWarning(
                    "Skipping Nukta record because commodity name is empty.");

                skipped++;

                continue;
            }
            if (string.IsNullOrWhiteSpace(item.Location))
            {
                _logger.LogWarning("Skipping commodity {Commodity} because location is empty.", item.Name);
                skipped++;

                continue;
            }

            var commodity = await GetOrCreateCommodityAsync(item,cancellationToken);
            var market =await GetOrCreateMarketAsync(item,cancellationToken);
            if (market == null)
            {
                _logger.LogWarning("Skipping {Commodity}. " +"Unable to create/find market for location {Location}.",item.Name,item.Location);
                skipped++;
                continue;
            }
            DateTimeOffset recordedAt;
            if (item.Date == default)
            {
                recordedAt = new DateTimeOffset(date ?? DateTime.UtcNow);
            }
            else
            {
                recordedAt = new DateTimeOffset(item.Date);
            }
            var existing =await _context.Prices.FirstOrDefaultAsync(x =>
                            x.CommodityId == commodity.Id &&
                            x.MarketId == market.Id &&
                            x.SourceId == source.Id &&
                            x.PriceType == PriceType.GENERAL &&
                            x.RecordedAt.Date == recordedAt.Date,
                        cancellationToken);

            if (existing != null)
            {
                existing.Price = item.Price;
                existing.MinPrice = item.MinPrice;
                existing.MaxPrice = item.MaxPrice;
                existing.Curreny = CurrenyEnum.TZS;
                updated++;
                continue;
            }           
            var price = new PriceModel
            {
                Price = item.Price,
                MinPrice = item.MinPrice,
                MaxPrice = item.MaxPrice,
                Curreny = CurrenyEnum.TZS,
                PriceType = PriceType.GENERAL,
                RecordedAt = recordedAt,
                SourceId = source.Id,
                CommodityId = commodity.Id,
                MarketId = market.Id
            };

            _context.Prices.Add(price);
            imported++;
        }
         await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Nukta import completed. " +"Inserted: {Imported}, Updated: {Updated}, Skipped: {Skipped}",imported,updated,skipped);
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
        _logger.LogInformation("Created price source: Nukta AI");
        return source;
    }

    

    private async Task<MarketModel?> GetOrCreateMarketAsync(NuktaCommodityPriceDto item,CancellationToken cancellationToken)
    {
        var location =item.Location?.Trim();

        if (string.IsNullOrWhiteSpace(location))
            return null;
        var market = await _context.Markets.Include(x => x.District).FirstOrDefaultAsync(x => x.Name == location,cancellationToken);
        if (market != null)
            return market;
        var district =await _context.Districts.FirstOrDefaultAsync(x => x.Name == location,cancellationToken);
        if (district == null)
        {
            district = new DistrictModel
            {
                Name = location
            };

            _context.Districts.Add(district);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Created district: {District}",location);
        }

        market = new MarketModel
        {
            Name = location,
            DistrictId = district.Id
        };
        _context.Markets.Add(market);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Created market: {Market}",location);

        return market;
    }
    private async Task<CommodityModel> GetOrCreateCommodityAsync(NuktaCommodityPriceDto item,CancellationToken cancellationToken)
    {
        var name =item.Name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("Commodity name cannot be empty.");
        var commodity = await _context.Commodities.FirstOrDefaultAsync(x => x.Name == name,cancellationToken);
        if (commodity != null)
            return commodity;
        commodity = new CommodityModel
        {
            Name = name,
            Unit = UnitEnum.KG
        };
        _context.Commodities.Add(commodity);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Created commodity: {Commodity}",name);

        return commodity;
    }
}