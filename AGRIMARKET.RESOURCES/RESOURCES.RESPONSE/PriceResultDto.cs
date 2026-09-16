using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.RESOURCES.RESOURCES.RESPONSE;

public class PriceResultDto
{
    public long Id { get; set; }

    public string? Commodity { get; set; }

    public string? Market { get; set; }

    public string? District { get; set; }

    public string? Region { get; set; }

    public string? Unit { get; set; }

    public decimal Price { get; set; }

    public decimal MinPrice { get; set; }

    public decimal MaxPrice { get; set; }

    public string? Currency { get; set; }

    public string? PriceType { get; set; }

    public DateTimeOffset RecordedAt { get; set; }
}
