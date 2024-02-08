using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Sfinx.Maui.Applink.Services;

namespace Sfinx.Maui.Applink
{
    [IntentFilter(new[] { Intent.ActionView },
            Categories = new[]
            {
                Intent.ActionView,
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
            }, AutoVerify = true,
            DataScheme = "sfinx", DataHost = "app"
        )
    ]
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                               ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density
    )]
    public class MainActivity : MauiAppCompatActivity
    {
        
    }
}