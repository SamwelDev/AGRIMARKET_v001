//using AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;
//using AGRIMARKET.RESOURCES.RESOURCES.EXT.EX.NUKTA;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Net.Http.Json;
//using System.Text;
//using System.Text.Json;

//namespace AGRIMARKET.INFRASTRUCTURE.INFRA.SV;

//public  class NuktaConfigurationClient : INuktaConfigurationClieant
//{
//    private readonly HttpClient _httpClient;
//    private readonly IConfiguration _configuration;

//    public NuktaConfigurationClient(HttpClient httpClient,IConfiguration configuration)
//    {
//        _httpClient = httpClient;
//        _configuration = configuration;
//    }

//    public async Task<IReadOnlyList<NuktaCommodityPriceDto>> GetCommodityPricesAsync(DateTime? date = null,CancellationToken cancellationToken = default)
//    {
//        var endpoint = "api/commodities";

//        if (date.HasValue)
//        {
//            endpoint += $"?date={date.Value:yyyy-MM-dd}";
//        }

//        var response = await _httpClient.GetAsync(
//            endpoint,
//            cancellationToken);

//        response.EnsureSuccessStatusCode();

//        var result =
//            await response.Content.ReadFromJsonAsync<NuktaCommodityResponseDto>(
//                cancellationToken);

//        return result?.Data?.Nafaka ?? [];
//    }

//}
using AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;
using AGRIMARKET.RESOURCES.RESOURCES.EXT.EX.NUKTA;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.SV;

public class NuktaConfigurationClient : INuktaConfigurationClieant
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<NuktaConfigurationClient> _logger;

    public NuktaConfigurationClient(HttpClient httpClient,IConfiguration configuration,ILogger<NuktaConfigurationClient> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IReadOnlyList<NuktaCommodityPriceDto>> GetCommodityPricesAsync(DateTime? date = null,CancellationToken cancellationToken = default)
    {
        var endpoint = "api/commodities";
        if (date.HasValue)
        {
            endpoint += $"?date={date.Value:yyyy-MM-dd}";
        }
        _logger.LogInformation("Requesting Nukta commodity prices. Endpoint: {Endpoint}",endpoint);
        using var response = await _httpClient.GetAsync(endpoint,cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogInformation("Nukta response status: {StatusCode}",response.StatusCode);
        response.EnsureSuccessStatusCode();
        if (string.IsNullOrWhiteSpace(json))
        {
            _logger.LogWarning("Nukta returned an empty response.");

            return [];
        }

        try
        {
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            if (root.TryGetProperty("success", out var successElement) &&
                successElement.ValueKind == JsonValueKind.False)
            {
                _logger.LogWarning("Nukta returned success=false.");

                return [];
            }
            if (!root.TryGetProperty("data", out var dataElement))
            {
                _logger.LogWarning("Nukta response does not contain 'data'.");

                return [];
            }
            if (dataElement.ValueKind == JsonValueKind.Array)
            {
                _logger.LogInformation("Nukta returned no commodity data for {Date}.",date?.ToString("yyyy-MM-dd") ?? "latest");

                return [];
            }
            if (dataElement.ValueKind == JsonValueKind.Object)
            {
                var data =JsonSerializer.Deserialize<NuktaCommodityDataDto>(dataElement.GetRawText(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                var prices = data?.Nafaka ?? []; _logger.LogInformation("Nukta returned {Count} commodity records.",
                    prices.Count);

                return prices;
            }

            _logger.LogWarning(
                "Unexpected Nukta data type: {DataType}",
                dataElement.ValueKind);

            return [];
        }
        catch (JsonException ex)
        {
            _logger.LogError(
                ex,
                "Failed to parse Nukta response. Endpoint: {Endpoint}. Response: {Response}",
                endpoint,
                json);

            throw;
        }
    }
}