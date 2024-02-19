using System.Diagnostics;
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
#if IOS || MACCATALYST
					lifecycle.AddiOS(ios =>
					{
						ios.FinishedLaunching((app, data)
							=> HandleAppLink(app.UserActivity));

						ios.ContinueUserActivity((app, userActivity, handler)
							=> HandleAppLink(userActivity));

						if (OperatingSystem.IsIOSVersionAtLeast(13) || OperatingSystem.IsMacCatalystVersionAtLeast(13))
						{
							ios.SceneWillConnect((scene, sceneSession, sceneConnectionOptions)
								=> HandleAppLink(sceneConnectionOptions.UserActivities.ToArray()
									.FirstOrDefault(a => a.ActivityType == Foundation.NSUserActivityType.BrowsingWeb)));

							ios.SceneContinueUserActivity((scene, userActivity)
								=> HandleAppLink(userActivity));
						}
					});
#elif ANDROID
                lifecycle.AddAndroid(android =>
                {
                    android.OnCreate((activity, bundle) =>
                    {
                        var action = activity.Intent?.Action;
                        var data = activity.Intent?.Data?.ToString();
                        Trace.WriteLine($"Android create {data}");

                        if (action == Android.Content.Intent.ActionView && data is not null)
                        {
                            Trace.WriteLine($"Handle app link");
                            //activity.Finish();
                            Task.Run(() => HandleAppLink(data));
                        }
                    });
                });
#endif
            });

        builder.Services.AddMauiBlazorWebView();
        //builder.Services.AddSingleton<DeeplinkAppService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<WeatherForecastService>();

        return builder.Build();
    }

    static void HandleAppLink(string url)
    {
        Trace.WriteLine($"Handling app link {url}");
        if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
        {
            Trace.WriteLine($"Data : {uri}");
            var relativeUrl = url.Replace("sfinx://app/", "");
            Preferences.Set("requestedUrl", relativeUrl);
            Trace.WriteLine("Main activity starting");
            //App.Current?.SendOnAppLinkRequestReceived(uri);
        }
    }
}