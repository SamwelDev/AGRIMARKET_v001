using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AGRIMARKET.RESOURCES.RESOURCES.EXT.EX.NUKTA;

public  class NuktaCommodityPriceDto
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("unit")]
    public string? Unit { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("min_price")]
    public decimal MinPrice { get; set; }

    [JsonPropertyName("max_price")]
    public decimal MaxPrice { get; set; }

    [JsonPropertyName("weekly")]
    public decimal? Weekly { get; set; }

    [JsonPropertyName("monthly")]
    public decimal? Monthly { get; set; }

    [JsonPropertyName("yearly")]
    public decimal? Yearly { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("trends")]
    public List<NuktaTrendDto> Trends { get; set; } = [];

    [JsonPropertyName("fundamentals")]
    public NuktaFundamentalsDto? Fundamentals { get; set; }
}

