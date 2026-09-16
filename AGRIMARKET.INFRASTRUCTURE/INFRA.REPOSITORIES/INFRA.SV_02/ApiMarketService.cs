using AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICE.IS_02;
using AGRIMARKET.RESOURCES.RESOURCES.RESPONSE;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.INFRA.SV_02;

public class ApiMarketService : IApiMarketService
{
    private readonly HttpClient _http;

    public ApiMarketService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PriceResponseDto?> GetPricesAsync(int pageNum = 1,int pageSize = 100,CancellationToken cancellationToken = default)
    {
        return await _http.GetFromJsonAsync<PriceResponseDto>($"api/Prices/get-all-Prices?pageNum={pageNum}&pageSize={pageSize}",cancellationToken);
    }
}
