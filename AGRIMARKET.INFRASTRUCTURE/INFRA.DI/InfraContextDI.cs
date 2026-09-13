using AGRIMARKET.INFRASTRUCTURE.INFRA.CONTEXT;
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
        var dbConnection = configuration.GetConnectionString("");
        services.AddDbContext<AgriMarketContext>(opt => opt.UseSqlServer(dbConnection,useSplit =>
        {
            useSplit.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }));
        return services;
    }
}
