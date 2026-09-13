using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.STR;
using AGRIMARKET.RESOURCES.RESOURCES.DTOS.RESOURCES.HELPERS.HELPER.PAGINATION;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.APPLICATION.APPLICATION.IR.IR.STR;

public interface IGeoRepoistory
{
    #region[REGIONS]
    Task<PaginatedResult<RegionDto>> GetAllRegionsAsync(CancellationToken cancellation, int pageSize, int pageNum);
    Task<RegionDto> GetRegionByIdAsync(long Id);
    Task<long> AddNewRegionsAsync(RegionDto region, CancellationToken cancellation);
    Task DeleteRegionAsync(long Id, CancellationToken cancellation);
    #endregion

    #region[DISTRICTS]
    Task<DistictDto> GetDistrictByIdAsync(long Id);
    Task<PaginatedResult<DistictDto>> GetAllDistrictsAsync(CancellationToken cancellation, int pageSize, int pageNum);
    Task<long> AddNewDistrcictsAsync(DistictDto distcrict, CancellationToken cancellation);
    Task DeleteDsistrictAsync(long Id, CancellationToken cancellation);
    #endregion

}
