using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AGRIMARKET.RESOURCES.RESOURCES.EXT.EX.NUKTA;

public class NuktaTrendDto
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}
