using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.MKT;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.STR;

public class DistrictModel
{
    public long Id {  get; set; }
    public string? Name { get; set; }
    public long? RegionId { get; set; }

    // Nav property
    public RegionModel? Region { get; set; }

    //Collection
    public ICollection<MarketModel> Markets { get; set; } = new HashSet<MarketModel>();
}
