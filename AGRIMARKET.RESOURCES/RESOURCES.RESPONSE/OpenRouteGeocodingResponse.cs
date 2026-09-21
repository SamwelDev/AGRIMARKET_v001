using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.RESOURCES.RESOURCES.RESPONSE;

public class OpenRouteGeocodingResponse
{
    public List<OpenRouteFeature>? Features { get; set; }
}
public class OpenRouteFeature
{
    public OpenRouteGeometry? Geometry { get; set; }
}

public class OpenRouteGeometry
{
    public double[]? Coordinates { get; set; }
}