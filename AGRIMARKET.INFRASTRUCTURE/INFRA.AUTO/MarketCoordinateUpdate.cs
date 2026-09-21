using AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;
using AGRIMARKET.INFRASTRUCTURE.INFRA.CONTEXT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.AUTO;

public class MarketCoordinateUpdate : IMarketCoordinateUpdate
{
    private readonly AgriMarketContext _context;
    private readonly IRouteConfigurationClient _geocodingService;
    private readonly ILogger<MarketCoordinateUpdate> _logger;

    public MarketCoordinateUpdate(AgriMarketContext context,IRouteConfigurationClient geocodingService,ILogger<MarketCoordinateUpdate> logger)
    {
        _context = context;
        _geocodingService = geocodingService;
        _logger = logger;
    }

    public async Task<int> ImportMissingCoordinatesAsync(CancellationToken cancellationToken = default)
    {
        var markets = await _context.Markets
            .Include(x => x.District)
            .Where(x =>x.Latitude == null ||x.Longitude == null)
            .ToListAsync(cancellationToken);

        var updated = 0;
        foreach (var market in markets)
        {
            if (string.IsNullOrWhiteSpace(market.Name))
                continue;

            var location = $"{market.Name}, Tanzania";_logger.LogInformation("Geocoding market {MarketName}",market.Name);

            var coordinates =
                await _geocodingService.GetCoordinatesAsync(
                    location,
                    cancellationToken);

            if (coordinates == null)
            {
                _logger.LogWarning(
                    "Coordinates not found for {MarketName}",
                    market.Name);

                continue;
            }

            market.Latitude = coordinates.Value.Latitude;
            market.Longitude = coordinates.Value.Longitude;

            updated++;

            // Important for public geocoding services:
            await Task.Delay(
                TimeSpan.FromSeconds(1),
                cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return updated;
    }
}
