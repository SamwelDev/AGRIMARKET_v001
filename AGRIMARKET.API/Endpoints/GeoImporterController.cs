using AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AGRIMARKET.API.Endpoints
{
    [EnableRateLimiting("Windows-Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class GeoImporterController : ControllerBase
    {
        private readonly IMarketCoordinateUpdate coordinatess;
        private readonly ILogger<GeoImporterController> logger;

        public GeoImporterController(IMarketCoordinateUpdate cod,ILogger<GeoImporterController> _logger)
        {
            coordinatess = cod;
            logger = _logger;
        }

        [EnableRateLimiting("Windows-Policy")]
        [HttpPost]
        [Route("update-market")]
        public async Task<IActionResult> ImportCoordinateAsync(CancellationToken cancellation)
        {
            var coordintes = await coordinatess.ImportMissingCoordinatesAsync(cancellation);
            return Ok(coordintes);
        }
    }
}
