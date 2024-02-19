using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Sfinx.Maui.Applink.Services;
using Trace = System.Diagnostics.Trace;

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
        // Prevents MainActivitiy from being re-created on launching an intent (also makes it to where `OnNewIntent` will be called directly, if the app has already been loaded)
        , LaunchMode = LaunchMode.SingleTop
    )]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            Trace.WriteLine("On Create intent");

            base.OnCreate(savedInstanceState);

            HandleIntent(Intent);
        }

        protected override void OnNewIntent(Intent intent)
        {
            Trace.WriteLine("On new intent");

            base.OnNewIntent(intent);

            HandleIntent(intent);
        }

        private void HandleIntent(Intent intent)
        {
            if (intent?.Data != null)
            {
                Trace.WriteLine($"Data : {intent.DataString}");
                var data = intent.Data;
                var url = data.ToString().Replace("sfinx://app/", "");
                Trace.WriteLine($"Url : {url}");
                Preferences.Set("requestedUrl", url);
                Trace.WriteLine("Main activity starting");
                StartActivity(typeof(MainActivity));
            }
        }
    }
}