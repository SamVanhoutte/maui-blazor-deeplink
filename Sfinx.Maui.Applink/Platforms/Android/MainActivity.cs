using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Sfinx.Maui.Applink.Services;
using Trace = System.Diagnostics.Trace;

namespace Sfinx.Maui.Applink
{
    [Activity(
        Theme = "@style/Maui.SplashTheme",
        MainLauncher = true,
        Exported = true,
        ConfigurationChanges = ConfigChanges.ScreenSize
                               | ConfigChanges.Orientation 
                               | ConfigChanges.UiMode
                               | ConfigChanges.ScreenLayout
                               | ConfigChanges.SmallestScreenSize
                               | ConfigChanges.KeyboardHidden
                               | ConfigChanges.Density)]
    [IntentFilter(
        new string[] { Intent.ActionView },
        AutoVerify = true,
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "sfinx",
        DataHost = "app")]
    public class MainActivity : MauiAppCompatActivity
    {
        // protected override void OnCreate(Bundle? savedInstanceState)
        // {
        //     Trace.WriteLine("On Create intent");
        //
        //     base.OnCreate(savedInstanceState);
        //
        //     HandleIntent(Intent);
        // }
        //
        // protected override void OnNewIntent(Intent intent)
        // {
        //     Trace.WriteLine("On new intent");
        //
        //     base.OnNewIntent(intent);
        //
        //     HandleIntent(intent);
        // }
        //
        // private void HandleIntent(Intent intent)
        // {
        //     if (intent?.Data != null)
        //     {
        //         Trace.WriteLine($"Data : {intent.DataString}");
        //         var data = intent.Data;
        //         var url = data.ToString().Replace("sfinx://app/", "");
        //         Trace.WriteLine($"Url : {url}");
        //         Preferences.Set("requestedUrl", url);
        //         Trace.WriteLine("Main activity starting");
        //         StartActivity(typeof(MainActivity));
        //     }
        // }
    }
}