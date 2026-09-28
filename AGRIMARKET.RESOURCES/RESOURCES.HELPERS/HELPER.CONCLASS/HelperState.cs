
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.STR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.RESOURCES.RESOURCES.HELPERS.HELPER.CONCLASS;

public class HelperState
{
    public List<PriceDto> Prices { get; set; } = [];

    public List<RegionDto> Regions { get; set; } = [];

    public bool IsLoaded { get; set; }

    public DateTimeOffset LoadedAt { get; set; }
}
