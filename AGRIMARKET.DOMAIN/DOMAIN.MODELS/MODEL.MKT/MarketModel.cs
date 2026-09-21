using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.STR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.MKT;

public class MarketModel
{
    public long Id { get; set; }
    public string? Name { get; set; }

    //Fr keys 
    public long? DistrictId { get; set; }
    public DistrictModel? District { get; set; }
    //Coordinates 
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }

    //Collection
    public ICollection<PriceModel> Prices { get; set; } = new HashSet<PriceModel>();
}
