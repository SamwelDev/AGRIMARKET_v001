using AGRIMARKET.APPLICATION.APPLICATION.IR.IR.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.STR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AGRIMARKET.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private readonly IMktRepository mktRepository;
        private readonly ILogger<MarketController> logger;

        public MarketController(IMktRepository mkt,ILogger<MarketController> _logger)
        {
            mktRepository = mkt;
            logger = _logger;
        }
        [EnableRateLimiting("Windows-Policy")]
        [HttpGet]
        [Route("get-all-Markets")]
        public async Task<IActionResult> GetAllMarketss(CancellationToken cancellation, int pageNum, int pageSize)
        {
            var marketData = await mktRepository.GetAllMarketsAsync(cancellation, pageNum, pageSize);
            return Ok(marketData);
        }

  
        [EnableRateLimiting("Windows-Policy")]
        [HttpPost]
        [Route("add-New-market")]
        public async Task<IActionResult> AddNewDistrict([FromBody] MarketDto mkt, CancellationToken cancellation)
        {
            var mktData = await mktRepository.AddNewMarketAsync(mkt, cancellation);
            return Ok(mktData);
        }
    }
}
