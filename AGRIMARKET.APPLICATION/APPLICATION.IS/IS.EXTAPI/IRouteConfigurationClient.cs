using AGRIMARKET.RESOURCES.RESOURCES.RESPONSE;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;

public interface IRouteConfigurationClient
{
    Task<(double Latitude, double Longitude)?> GetCoordinatesAsync(string location, CancellationToken cancellationToken = default);
    Task<List<RouteDistanceResponse>> GetDistancesAsync(double originLatitude, double originLongitude, List<RouteDestinationRequest> destinations, CancellationToken cancellationToken = default);
}
