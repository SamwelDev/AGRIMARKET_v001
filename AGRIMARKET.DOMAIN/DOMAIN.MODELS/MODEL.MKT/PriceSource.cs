using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.MKT;

public class PriceSource
{
    public long Id  { get; set; }
    public string? Name { get; set; }
    //Collection
    public ICollection<PriceModel> Prices { get; set; } = new HashSet<PriceModel>();
}
