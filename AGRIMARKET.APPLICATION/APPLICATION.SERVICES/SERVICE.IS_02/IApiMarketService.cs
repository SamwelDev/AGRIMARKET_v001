using AGRIMARKET.RESOURCES.RESOURCES.RESPONSE;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICE.IS_02;

public interface IApiMarketService
{
    Task<PriceResponseDto?> GetPricesAsync(int pageNum = 1, int pageSize = 100, CancellationToken cancellationToken = default);
}
