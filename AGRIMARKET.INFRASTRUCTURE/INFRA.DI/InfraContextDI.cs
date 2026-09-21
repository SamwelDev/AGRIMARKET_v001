using AGRIMARKET.APPLICATION.APPLICATION.IR.IR.MKT;
using AGRIMARKET.APPLICATION.APPLICATION.IR.IR.STR;
using AGRIMARKET.APPLICATION.APPLICATION.IS.IS.EXTAPI;
using AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICE.IS_02;
using AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICES.IS;
using AGRIMARKET.INFRASTRUCTURE.INFRA.AUTO;
using AGRIMARKET.INFRASTRUCTURE.INFRA.AUTO.AUTO.BACKGROUND;
using AGRIMARKET.INFRASTRUCTURE.INFRA.CONTEXT;
using AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.INFRA.SV_02;
using AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.REPOSITORY.MKT;
using AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.RESPOSITORY.STR;
using AGRIMARKET.INFRASTRUCTURE.INFRA.SV;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.DI;

public static class InfraContextDI
{
    public static IServiceCollection AddInfraDI(this IServiceCollection services,IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("DefaultDbDev");
        services.AddDbContext<AgriMarketContext>(opt => opt.UseSqlServer(dbConnection,useSplit =>
        {
            useSplit.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }));
        //::Repositories CLUSTER:01
        services.AddScoped<IGeoRepoistory, GeoRepository>();
        services.AddScoped<IMktRepository, MktRepository>();

        // CLUSTER 02
        services.AddScoped<IExternalDataImpoter, ExternalDataImporterService>();
        // CLUSTER 03
        services.AddHttpClient<IApiMarketService, ApiMarketService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7064/");
        });
        // CLUSTER 04
        services.AddHttpClient<IRouteConfigurationClient, RouteConfigurationClient>();
        services.AddHostedService<ExternalImportBackground>();
        services.AddScoped<IMarketCoordinateUpdate, MarketCoordinateUpdate>();
        return services;
    }
}
