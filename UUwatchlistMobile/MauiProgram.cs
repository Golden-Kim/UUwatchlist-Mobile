using Microsoft.Extensions.Logging;
using UUwatchlistMobile.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;
namespace UUwatchlistMobile
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

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UUwatchlistMobile.appsettings.json");

            if (stream != null)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
                builder.Configuration.AddConfiguration(config);
            }


            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<YoutubeService>();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<ApiKeyService>();
            builder.Services.AddSingleton<YoutubePollingService>();
            builder.Services.AddSingleton<FandomApi>();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
