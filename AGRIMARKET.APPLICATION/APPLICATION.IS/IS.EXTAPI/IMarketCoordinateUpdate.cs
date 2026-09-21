using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;

public interface IMarketCoordinateUpdate
{
    Task<int> ImportMissingCoordinatesAsync(CancellationToken cancellationToken = default);
}
