using Microsoft.Extensions.Logging;
using UUWatchlist.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using UUwatchlistMobile.Services;
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
            builder.Services.AddSingleton<SqlService>();
            builder.Services.AddSingleton<ApiKeyService>();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
