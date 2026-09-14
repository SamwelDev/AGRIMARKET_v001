using AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;
using AGRIMARKET.RESOURCES.RESOURCES.EXT.EX.NUKTA;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

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

    public async Task<IReadOnlyList<NuktaCommodityPriceDto>>GetCommodityPricesAsync(DateTime? date = null,CancellationToken cancellationToken = default)
    {
        var endpoint =_configuration["Nukta:CommodityEndpoint"];
        if (string.IsNullOrWhiteSpace(endpoint))
            throw new InvalidOperationException(
                "Nukta commodity endpoint is not configured.");

        var url = endpoint;

        if (date.HasValue)
        {
            url += $"?date={date.Value:yyyy-MM-dd}";
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            url);

        var apiKey = _configuration["Nukta:ApiKey"];

        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            request.Headers.Add(
                "X-API-Key",
                apiKey);
        }

        var response = await _httpClient.SendAsync(
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<List<NuktaCommodityPriceDto>>(cancellationToken: cancellationToken);
        return data ?? [];
    }
}
