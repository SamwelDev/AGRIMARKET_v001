using AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICES.IS;
using AGRIMARKET.RESOURCES.RESOURCES.EXT.EX.NUKTA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AGRIMARKET.API.Endpoints
{
    [EnableRateLimiting("Windows-Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExImporterController : ControllerBase
    {
        private readonly IExternalDataImpoter externalDataImpoter;
        private readonly ILogger<ExImporterController> logger;

        public ExImporterController(IExternalDataImpoter Excontrollers,ILogger<ExImporterController> loggers)
        {
            externalDataImpoter = Excontrollers;
            logger = loggers;
        }
        [EnableRateLimiting("Windows-Policy")]
        [HttpPost]
        [Route("import-prices-data")]
        public async Task<IActionResult> ImportallDataAPI([FromQuery] DateTime? date = null, CancellationToken cancellationToken = default)
        {
            var import = await externalDataImpoter.ImportPricesAsync(date,cancellationToken);
            return Ok(import);
            
        }

    }
}
