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
//Route calculation ..
public class RouteDestinationRequest
{
    public long MarketId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
public class RouteDistanceResponse
{
    public long MarketId { get; set; }
    public double DistanceKm { get; set; }
    public double DurationMinutes { get; set; }
}

public class RouteMatrixResponse
{
    public double?[][]? Distances { get; set; }

    public double?[][]? Durations { get; set; }
}
