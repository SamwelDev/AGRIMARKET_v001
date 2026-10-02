using AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICE.IS_02;
using AGRIMARKET.INFRASTRUCTURE.INFRA.DI;
using AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.INFRA.SV_02;
using AGRIMARKET.RESOURCES.RESOURCES.HELPERS.HELPER.CONCLASS;
using AGRIMARKET.RESOURCES.RESOURCES.HELPERS.RESOURCES.THEMES;
using AGRIMARKET.WEB;
using AGRIMARKET.WEB.Layout;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(opt => new HttpClient
{
    BaseAddress = new Uri("https://192.168.0.119:7064/")
}); 
builder.Services.AddScoped<IApiMarketService, ApiMarketService>();
builder.Services.AddScoped<ThemeWrapper>();
builder.Services.AddScoped<HelperState>();
await builder.Build().RunAsync();
