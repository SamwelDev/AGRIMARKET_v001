using AGRIMARKET.RESOURCES.RESOURCE.ENUMS;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.MKT;

public class CommodityModel
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public UnitEnum Unit { get; set; } = UnitEnum.KG;

    public ICollection<PriceModel> Prices { get; set; } = new HashSet<PriceModel>();
}
