using AGRIMARKET.RESOURCES.RESOURCES.DTOS.DTO.MKT;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.RESOURCES.RESOURCES.RESPONSE;

public  class PriceResponseDto
{
    public List<PriceDto> Data { get; set; } = new();
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int PageNum { get; set; }
    public int PageCount { get; set; }
}
