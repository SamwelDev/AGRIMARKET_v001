using AGRIMARKET.RESOURCES.RESOURCES.EXT.EX.NUKTA;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;

public  interface INuktaConfigurationClieant
{
    Task<IReadOnlyList<NuktaCommodityPriceDto>> GetCommodityPricesAsync(DateTime? date = null, CancellationToken cancellationToken = default);
}
