using AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;
using AGRIMARKET.RESOURCES.RESOURCES.EXT.EX.NUKTA;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.SV;

public  class NuktaConfigurationClient : INuktaConfigurationClieant
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public NuktaConfigurationClient(HttpClient httpClient,IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<IReadOnlyList<NuktaCommodityPriceDto>> GetCommodityPricesAsync(
    DateTime? date = null,
    CancellationToken cancellationToken = default)
    {
        var endpoint = "api/commodities";

        if (date.HasValue)
        {
            endpoint += $"?date={date.Value:yyyy-MM-dd}";
        }

        var response = await _httpClient.GetAsync(
            endpoint,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<NuktaCommodityResponseDto>(
            cancellationToken);

        return result?.Data?.Nafaka ?? [];
    }

}
