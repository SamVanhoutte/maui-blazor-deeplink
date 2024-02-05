using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Sfinx.Maui.Applink.Data;
using Sfinx.Maui.Applink.Services;

namespace Sfinx.Maui.Applink;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); })
            .ConfigureLifecycleEvents(lifecycle =>
            {
                // Just test Android
                lifecycle.AddAndroid(android => {
                    android.OnCreate((activity, bundle) =>
                    {
                        var action = activity.Intent?.Action;
                        var data = activity.Intent?.Data?.ToString();

                        if (action == Android.Content.Intent.ActionView && data is not null)
                        {
                            activity.Finish();
                            HandleAppLink(data);
//                            System.Threading.Tasks.Task.Run(() => HandleAppLink(data));
                        }
                    });
                });      
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddSingleton<DeeplinkAppService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<WeatherForecastService>();

        return builder.Build();
    }
    
    static void HandleAppLink(string url)
    {
        if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
        {
            App.Current?.SendOnAppLinkRequestReceived(uri);
        }
    }
}