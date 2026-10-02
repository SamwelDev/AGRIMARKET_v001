using AGRIMARKET.APPLICATION.APPLICATION.SERVICES.SERVICE.IS_02;
using AGRIMARKET.INFRASTRUCTURE.INFRA.REPOSITORIES.INFRA.SV_02;
using AGRIMARKET.RESOURCES.RESOURCES.HELPERS.HELPER.CONCLASS;
using AGRIMARKET.RESOURCES.RESOURCES.HELPERS.RESOURCES.THEMES;
using Microsoft.Extensions.Logging;

namespace AGRIMARKET.MOBILE
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddScoped(_ =>
            {
                var handler = new HttpClientHandler();

#if DEBUG
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#endif

                return new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://192.168.0.119:7064/")
                };
            });

            builder.Services.AddScoped<IApiMarketService, ApiMarketService>();
            builder.Services.AddScoped<ThemeWrapper>();
            builder.Services.AddScoped<HelperState>();
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
            return builder.Build();
        }
    }
}
