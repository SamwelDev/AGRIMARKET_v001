using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AGRIMARKET.RESOURCES.RESOURCES.EXT.EX.NUKTA;

public  class NuktaFundamentalsDto
{
    [JsonPropertyName("as_of_date")]
    public DateTime AsOfDate { get; set; }

    [JsonPropertyName("return_1w_pct")]
    public decimal? Return1wPct { get; set; }

    [JsonPropertyName("return_1m_pct")]
    public decimal? Return1mPct { get; set; }

    [JsonPropertyName("return_3m_pct")]
    public decimal? Return3mPct { get; set; }

    [JsonPropertyName("return_6m_pct")]
    public decimal? Return6mPct { get; set; }

    [JsonPropertyName("return_ytd_pct")]
    public decimal? ReturnYtdPct { get; set; }

    [JsonPropertyName("return_1y_pct")]
    public decimal? Return1yPct { get; set; }

    [JsonPropertyName("return_3y_pct")]
    public decimal? Return3yPct { get; set; }


    [JsonPropertyName("return_5y_pct")]
    public decimal? Return5yPct { get; set; }

    [JsonPropertyName("volatility_ann_pct")]
    public decimal? VolatilityAnnPct { get; set; }

    [JsonPropertyName("trailing_1y_high")]
    public decimal? Trailing1yHigh { get; set; }

    [JsonPropertyName("trailing_1y_low")]
    public decimal? Trailing1yLow { get; set; }
}
public class NuktaCommodityDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal? Weekly { get; set; }
    public decimal? Monthly { get; set; }
    public decimal? Yearly { get; set; }

    public List<NuktaTrendDto> Trends { get; set; } = [];

    public NuktaFundamentalsDto? Fundamentals { get; set; }

    public string Category { get; set; } = string.Empty;
}
public class NuktaFiltersDto
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("date")]
    public string? Date { get; set; }
}
public class NuktaMetaDto
{
    [JsonPropertyName("last_updated")]
    public string? LastUpdated { get; set; }

    [JsonPropertyName("last_updated_formatted")]
    public string? LastUpdatedFormatted { get; set; }

    [JsonPropertyName("latest_date")]
    public string? LatestDate { get; set; }

    [JsonPropertyName("unit")]
    public string? Unit { get; set; }

    [JsonPropertyName("filters")]
    public NuktaFiltersDto? Filters { get; set; }
}
public class NuktaCommodityResponseDto
{
    
  
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public NuktaCommodityDataDto? Data { get; set; }

    [JsonPropertyName("meta")]
    public NuktaMetaDto? Meta { get; set; }
}
public class NuktaCommodityDataDto
{
    [JsonPropertyName("Nafaka")]
    public List<NuktaCommodityPriceDto> Nafaka { get; set; } = [];
}