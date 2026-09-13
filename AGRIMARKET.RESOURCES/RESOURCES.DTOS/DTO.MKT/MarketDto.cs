using AGRIMARKET.RESOURCES.RESOURCE.ENUMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;

public  class MarketDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public long? DistrictId { get; set; }
    public ICollection<PriceDto> Prices { get; set; } = new HashSet<PriceDto>();
}
public class PriceDto
{
    public long Id { get; set; }
    public decimal Price { get; set; }
    public CurrenyEnum Curreny { get; set; } = CurrenyEnum.TZS;
    public PriceType PriceType { get; set; } = PriceType.RETAIL;
    public DateTimeOffset RecordedAt { get; set; } = DateTimeOffset.Now;
    public long SourceId { get; set; }
    public long? CommodityId { get; set; }
    public long? MarketId { get; set; }
    public CommodityDto? Commodity { get; set; }

}
public class SourceDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    //Collection
    public ICollection<PriceDto> Prices { get; set; } = new HashSet<PriceDto>();
}
public class CommodityDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public UnitEnum Unit { get; set; } = UnitEnum.KG;

    public ICollection<PriceDto> Prices { get; set; } = new HashSet<PriceDto>();

}