using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.STR
{
    public class RegionModel
    {
        public long Id {  get; set; }
        public string? Name { get; set; }

        //Collection
        public ICollection<DistrictModel> Districts { get; set; } = new HashSet<DistrictModel>();
    }
}
