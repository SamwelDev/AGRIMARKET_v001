using AGRIMARKET.APPLICATION.APPLICATION.IR.IR.MKT;
using AGRIMARKET.APPLICATION.APPLICATION.IR.IR.STR;
using AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICES.IS;
using AGRIMARKET.INFRASTRUCTURE.INFRA.AUTO;
using AGRIMARKET.INFRASTRUCTURE.INFRA.CONTEXT;
using AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.REPOSITORY.MKT;
using AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.RESPOSITORY.STR;
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
        return services;
    }
}
