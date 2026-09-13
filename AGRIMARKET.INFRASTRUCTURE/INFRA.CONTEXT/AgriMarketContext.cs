using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.MKT;
using AGRIMARKET.DOMAIN.DOMAIN.MODELS.MODEL.STR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.CONTEXT;

public class AgriMarketContext : DbContext
{
    public AgriMarketContext(DbContextOptions<AgriMarketContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    //DbSets ..
    public DbSet<RegionModel> Regions { get; set; }
    public DbSet<DistrictModel> Districts { get; set; }
    //Markets
    public DbSet<MarketModel> Markets { get; set; }
    public DbSet<PriceModel> Prices { get; set; }
    public DbSet<CommodityModel> Commodities { get; set; }
    public DbSet<PriceSource> Sources { get; set; }
}
