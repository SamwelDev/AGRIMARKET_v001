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
    public class CommodititesController : ControllerBase
    {
        private readonly IMktRepository mktRepository;
        private readonly ILogger<MarketController> logger;

        public CommodititesController(IMktRepository mkt, ILogger<MarketController> _logger)
        {
            mktRepository = mkt;
            logger = _logger;
        }
        [EnableRateLimiting("Windows-Policy")]
        [HttpGet]
        [Route("get-all-Commodities")]
        public async Task<IActionResult> GetAllCommodities(CancellationToken cancellation, int pageNum, int pageSize)
        {
            var marketData = await mktRepository.GetAllCommoditiesAsync(cancellation, pageNum, pageSize);
            return Ok(marketData);
        }
        [EnableRateLimiting("Windows-Policy")]
        [HttpGet]
        [Route("get-by-Id/{Id:long}")]
        public async Task<IActionResult> GetCommodityById(long Id)
        {
            var cmdData = await mktRepository.GetCommodityByIdAsync(Id);
           return  Ok(cmdData);
        }

        [EnableRateLimiting("Windows-Policy")]
        [HttpPost]
        [Route("add-New-commodity")]
        public async Task<IActionResult> AddNewDistrict([FromBody] CommodityDto mkt, CancellationToken cancellation)
        {
            var mktData = await mktRepository.AddNewCommodityAsync(mkt, cancellation);
            return Ok(mktData);
        }
    }
}
