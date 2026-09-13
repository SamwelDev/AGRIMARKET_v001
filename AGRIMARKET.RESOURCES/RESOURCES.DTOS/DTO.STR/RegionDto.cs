using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.STR;

public  class RegionDto
{
    public long Id { get; set; }
    public string? Name { get; set; }

    //Collection
    public ICollection<DistictDto> Districts { get; set; } = new HashSet<DistictDto>();
}
public class DistictDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    //Collection
    public ICollection<MarketDto> Markets { get; set; } = new HashSet<MarketDto>();
}
