using AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICE.IS_02;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.STR;
using AGRIMARKET.RESOURCES.RESOURCES.HELPERS.HELPER.PAGINATION;
using AGRIMARKET.RESOURCES.RESOURCES.RESPONSE;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.INFRA.SV_02;

public class ApiMarketService : IApiMarketService
{
    private readonly HttpClient _http;

    public ApiMarketService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PaginatedResult<PriceDto>?> GetPricesAsync(int pageNum = 1,int pageSize = 100,CancellationToken cancellationToken = default)
    {
        var url = $"api/Prices/get-all-Prices?pageNum={pageNum}&pageSize={pageSize}";
        var result = await _http.GetFromJsonAsync<PaginatedResult<PriceDto>>(url,cancellationToken);
       return result;
    }
    public async Task<PaginatedResult<RegionDto>?> GetRegionsAsync(int pageNum =1,int pageSize=50, CancellationToken cancellationToken = default)
    {
        var _url = $"api/Geo/get-all-regions?pageNum={pageNum}&pageSize={pageSize}";
        var data = await _http.GetFromJsonAsync<PaginatedResult<RegionDto>>(_url, cancellationToken);
        return data;
    }
    public async Task<List<ProfitDto>> FindProfitableMarketsAsync(long commodityId,long originMarketId,decimal quantity,CancellationToken cancellationToken = default)
    {
        var request = new FindProfitableMarketsRequest
        {
            CommodityId = commodityId,
            OriginMarketId = originMarketId,
            Quantity = quantity
        };

        using var response = await _http.PostAsJsonAsync(
            "api/Market/find-profitable-market",
            request,
            cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Market profitability request failed. " +
                $"Status: {(int)response.StatusCode} {response.StatusCode}. " +
                $"Response: {responseBody}");
        }

        var result = JsonSerializer.Deserialize<List<ProfitDto>>(
            responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return result ?? [];
    }
}
