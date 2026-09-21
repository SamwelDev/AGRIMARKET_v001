using AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;
using AGRIMARKET.RESOURCES.RESOURCES.RESPONSE;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.SV;

public class RouteConfigurationClient : IRouteConfigurationClient
{
    private readonly HttpClient httpClient;
    private readonly ILogger<RouteConfigurationClient> logger;
    private readonly IConfiguration config;

    public RouteConfigurationClient(HttpClient client,ILogger<RouteConfigurationClient> _logger,IConfiguration configuration)
    {
        httpClient = client;
        logger = _logger;
        config = configuration;
    }
    public async Task<(double Latitude, double Longitude)?> GetCoordinatesAsync( string location, CancellationToken cancellationToken = default)
    {
        var apiKey = config["RouteService:apiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "OpenRouteService API key is not configured.");
        }

        var url = $"https://api.openrouteservice.org/geocode/search" + $"?api_key={Uri.EscapeDataString(apiKey)}" + $"&text={Uri.EscapeDataString(location)}" + $"&size=1";

        using var response = await httpClient.GetAsync(
            url,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning(
                "Geocoding failed for {Location}. Status: {StatusCode}",
                location,
                response.StatusCode);

            return null;
        }
        var result =await response.Content.ReadFromJsonAsync<OpenRouteGeocodingResponse>(cancellationToken);
        var feature = result?.Features?.FirstOrDefault();
        if (feature?.Geometry?.Coordinates == null ||
            feature.Geometry.Coordinates.Length < 2)
        {
            return null;
        }
        var longitude = feature.Geometry.Coordinates[0];
        var latitude = feature.Geometry.Coordinates[1];
        return (latitude, longitude);
    }
}
