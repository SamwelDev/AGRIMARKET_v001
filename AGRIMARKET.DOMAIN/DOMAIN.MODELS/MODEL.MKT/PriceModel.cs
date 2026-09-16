using AGRIMARKET.RESOURCES.RESOURCE.ENUMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.MKT;

public class PriceModel
{
    public long Id { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal MinPrice { get; set; }
    public decimal Price { get; set; }
    public CurrenyEnum Curreny { get; set; } = CurrenyEnum.TZS;
    public PriceType PriceType { get; set; } = PriceType.RETAIL;
    public DateTimeOffset RecordedAt { get; set; } 

    //Fr keys
    public long SourceId { get; set; }
    public long? CommodityId { get; set; }
    public long? MarketId { get; set; }
    public PriceSource? PriceSource { get; set; }
    public CommodityModel? Commodity { get; set; }
    public MarketModel? Market { get; set; }


}
