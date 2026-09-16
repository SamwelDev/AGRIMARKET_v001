using AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICE.IS_02;
using AGRIMARKET.INFRASTRUCTURE.INFRA.DI;
using AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.INFRA.SV_02;
using AGRIMARKET.WEB;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(opt => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7064/")
}); builder.Services.AddScoped<IApiMarketService, ApiMarketService>();
await builder.Build().RunAsync();
