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
            throw new InvalidOperationException("OpenRouteService API key is not configured.");
        }
        var url = $"https://api.openrouteservice.org/geocode/search" + $"?api_key={Uri.EscapeDataString(apiKey)}" + $"&text={Uri.EscapeDataString(location)}" + $"&size=1";
        using var response = await httpClient.GetAsync(url,cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Geocoding failed for {Location}. Status: {StatusCode}",location,response.StatusCode);
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
    public async Task<List<RouteDistanceResponse>> GetDistancesAsync(double originLatitude,double originLongitude,List<RouteDestinationRequest> destinations,CancellationToken cancellationToken = default)
    {
        var apiKey = config["RouteService:apiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "OpenRouteService API key is not configured.");
        }

        if (destinations == null || destinations.Count == 0)
        {
            return new List<RouteDistanceResponse>();
        }

       
     var locations = new List<double[]>
    {
        new double[]
        {
            originLongitude,
            originLatitude
        }
    };

        foreach (var destination in destinations)
        {
            locations.Add(
                new double[]
                {
                destination.Longitude,
                destination.Latitude
                });
        }

      
        var requestBody = new
        {
            locations = locations,

            sources = new[]
            {
            "0"
        },

            destinations = Enumerable
                .Range(1, destinations.Count)
                .Select(x => x.ToString())
                .ToArray(),

            metrics = new[]
            {
            "distance",
            "duration"
        }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post,"https://api.openrouteservice.org/v2/matrix/driving-car");
        request.Headers.TryAddWithoutValidation("Authorization",apiKey);
        request.Content = JsonContent.Create(requestBody);
        using var response = await httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.LogWarning(
                "Route matrix failed. Status: {StatusCode}, Error: {Error}",
                response.StatusCode,
                error);

            return new List<RouteDistanceResponse>();
        }
        var result = await response.Content.ReadFromJsonAsync<RouteMatrixResponse>(cancellationToken);
        var results = new List<RouteDistanceResponse>();
        if (result?.Distances == null ||result.Durations == null || result.Distances.Length == 0 ||result.Durations.Length == 0)
        {
            return results;
        }
        for (var i = 0; i < destinations.Count; i++)
        {
            var distanceMeters = result.Distances[0]?[i];
            var durationSeconds = result.Durations[0]?[i];
            if (!distanceMeters.HasValue ||!durationSeconds.HasValue)
            {
                logger.LogWarning("No route found from origin to MarketId {MarketId}",destinations[i].MarketId);
                continue;
            }

            results.Add(new RouteDistanceResponse
            {
                MarketId = destinations[i].MarketId,
                DistanceKm = distanceMeters.Value / 1000.0,
                DurationMinutes = durationSeconds.Value / 60.0
            });
        }

        return results;
    }
}
