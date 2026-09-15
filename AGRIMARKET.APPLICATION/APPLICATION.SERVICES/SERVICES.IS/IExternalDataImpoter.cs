using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.EXT.EX.NUKTA;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICES.IS;

public  interface IExternalDataImpoter
{
    //Data importer fo
    Task<int> ImportPricesAsync(DateTime? date = null, CancellationToken cancellationToken = default);
}
