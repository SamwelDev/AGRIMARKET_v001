using AGRIMARKET.APPLICATION.APPLICATION.IR.IR.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AGRIMARKET.API.Endpoints
{
    [EnableRateLimiting("Windows-Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly IMktRepository mktRepository;
        private readonly ILogger<PricesController> logger;

        public PricesController(IMktRepository proces,ILogger<PricesController> _logger)
        {
            logger = _logger;
            mktRepository = proces;
        }
        [EnableRateLimiting("Windows-Policy")]
        [HttpGet]
        [Route("get-all-Prices")]
        public async Task<IActionResult> GetAllPrices(CancellationToken cancellation,int pageNum,int pageSize)
        {
            var pricesData = await mktRepository.GetAllPricesAsync(cancellation,pageNum,pageSize);
            return Ok(pricesData);
        }

        [EnableRateLimiting("Windows-Policy")]
        [HttpGet]
        [Route("get-latest-price")]
        public async Task<IActionResult> GetLatestPrice(CancellationToken cancellation)
        {
            var dataDb = await mktRepository.GetLatestAsync(cancellation);
            if (dataDb == null)
                return NotFound("No data price found");
            return Ok(dataDb);
        }
        [EnableRateLimiting("Windows-Policy")]
        [HttpGet]
        [Route("history-price")]
        public async Task<IActionResult> GetHistoryPrices([FromQuery] DateTime? from,[FromQuery] DateTime? to,[FromQuery] int pageSize = 20,[FromQuery] int pageNum = 1, CancellationToken cancellation = default)
        {
            var historyPrice = await mktRepository.GetPriceHistoryAsync(from,to,cancellation,pageSize,pageNum);
            if (historyPrice == null)
                return NotFound("No history prcices");
            return Ok(historyPrice);
        }
        [EnableRateLimiting("Windows-Policy")]
        [HttpGet]
        [Route("add-new-Price")]
        public async Task<IActionResult> AddNewPrice(PriceDto dto,CancellationToken cancellation)
        {
            var newPrice = await mktRepository.AddNewPriceAsync(dto,cancellation);
            return Ok(newPrice);
        }

    }

}
