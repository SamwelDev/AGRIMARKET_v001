using AGRIMARKET.APPLICATION.APPLICATION.IR.IR.STR;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.STR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AGRIMARKET.API.Endpoints
{
    [EnableRateLimiting("Windows-Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class GeoController : ControllerBase
    {
        private readonly IGeoRepoistory geoRepoistory;
        private readonly ILogger<GeoController> logger;

        public GeoController(IGeoRepoistory geo,ILogger<GeoController> _logger)
        {
            geoRepoistory = geo;
            logger = _logger;

        }
        [EnableRateLimiting("Windows-Policy")]
        [HttpGet]
        [Route("get-all-regions")]
        public async Task<IActionResult> GetAllReagions(CancellationToken cancellation,int pageNum,int pageSize)
        {
            var regionData = await geoRepoistory.GetAllRegionsAsync(cancellation: cancellation,pageSize: pageSize,pageNum: pageNum);

            return Ok(regionData);
        }

        [EnableRateLimiting("Windows-Policy")]
        [HttpPost]
        [Route("add-New-Region")]
        public async Task<IActionResult> AddNewRegion([FromBody] RegionDto region,CancellationToken cancellation)
        {
            var regionData = await geoRepoistory.AddNewRegionsAsync(region, cancellation);
            return Ok(regionData);
        }
        [EnableRateLimiting("Windows-Policy")]
        [HttpGet]
        [Route("region-By-Id/{Id:long}")]
        public async Task<IActionResult> GetRegionById(long Id)
        {
            var districtData = await geoRepoistory.GetRegionByIdAsync(Id);
            return Ok(districtData);
        }
        [EnableRateLimiting("Windows-Policy")]
        [HttpGet]
        [Route("get-all-Distrcit")]
        public async Task<IActionResult> GetAllDistrcicts(CancellationToken cancellation, int pageNum, int pageSize)
        {
            var districtData = await geoRepoistory.GetAllDistrictsAsync(cancellation, pageNum, pageSize);
            return Ok(districtData);
        }

        [EnableRateLimiting("Windows-Policy")]
        [HttpGet]
        [Route("district-By-Id/{Id:long}")]
        public async Task<IActionResult> GetDistrictById(long Id)
        {
            var districtData = await geoRepoistory.GetDistrictByIdAsync(Id);
            return Ok(districtData);
        }
        [EnableRateLimiting("Windows-Policy")]
        [HttpPost]
        [Route("add-New-District")]
        public async Task<IActionResult> AddNewDistrict([FromBody] DistictDto distict, CancellationToken cancellation)
        {
            var regionData = await geoRepoistory.AddNewDistrcictsAsync(distict, cancellation);
            return Ok(regionData);
        }
        //[EnableRateLimiting("Windows-Policy")]
        //[HttpGet]
        //[Route("delete-District/{Id:long}")]
        //public async Task<IActionResult> DeleteDistrictData(CancellationToken cancellation,Id)
        //{
        //    var districtData = await geoRepoistory.DeleteDsistrictAsync(cancellation,Id);
        //    return Ok(districtData);
        //}

    }
}
