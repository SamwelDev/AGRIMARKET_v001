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
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public ICollection<PriceDto> Prices { get; set; } = new HashSet<PriceDto>();
}
public class PriceDto
{
    public long Id { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal MinPrice { get; set; }
    public decimal Price { get; set; }
    public CurrenyEnum Curreny { get; set; }
    public PriceType PriceType { get; set; }
    public DateTimeOffset RecordedAt { get; set; }
    public string? Commodity { get; set; }
    public string? Market { get; set; }
    public string? District { get; set; }
    public string? Region { get; set; }
    public string? Unit { get; set; }
    public string? Currency { get; set; }

    public long SourceId { get; set; }
    public long? CommodityId { get; set; }
    public long? MarketId { get; set; }
    //XTra
  
    public decimal CurrentPrice { get; set; }
    public decimal Share { get; set; }
    public string Color { get; set; } = "";

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

public class CompareDto
{
    public string? PriceType { get; set; }

    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public decimal PriceDifference { get; set; }
    public decimal? PricePercentageChange { get; set; }
    public decimal OldMinPrice { get; set; }
    public decimal NewMinPrice { get; set; }
    public decimal MinPriceDifference { get; set; }
    public decimal? MinPricePercentageChange { get; set; }
    public decimal OldMaxPrice { get; set; }
    public decimal NewMaxPrice { get; set; }
    public decimal MaxPriceDifference { get; set; }
    public decimal? MaxPricePercentageChange { get; set; }
    public DateTimeOffset OldRecordedAt { get; set; }
    public DateTimeOffset NewRecordedAt { get; set; }
}