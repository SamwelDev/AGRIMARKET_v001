using AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICES.IS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.INFRASTRUCTURE.INFRA.AUTO.AUTO.BACKGROUND;

public class ExternalImportBackground : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ExternalImportBackground> _logger;

    public ExternalImportBackground(IServiceScopeFactory scopeFactory,ILogger<ExternalImportBackground> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("background service is on ..");
        _logger.LogInformation("Data import worker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var tanzaniaTime =TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow,"E. Africa Standard Time");
                var date = tanzaniaTime.Date;
                var importer =scope.ServiceProvider.GetRequiredService<IExternalDataImpoter>();
                var imported =await importer.ImportPricesAsync(date,stoppingToken); 
                _logger.LogInformation("Data import completed. Imported: {Count}",imported);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Data automatic import failed.");
            }
            await Task.Delay(TimeSpan.FromHours(24),stoppingToken);
        }
    }
}
