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
        // Prevents MainActivitiy from being re-created on launching an intent (also makes it to where `OnNewIntent` will be called directly, if the app has already been loaded)
        , LaunchMode = LaunchMode.SingleTop
    )]
    public class MainActivity : MauiAppCompatActivity
    {
        private readonly DeeplinkAppService deeplinkService;

        public MainActivity()
        {
            deeplinkService =
                ServiceHelper.Current
                    .GetRequiredService<DeeplinkAppService>(); 
        }
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            OnNewIntent(this.Intent);

            // var intent = Intent;
            // if (intent != null)
            // {
            //     if (intent.Action == Intent.ActionView && !string.IsNullOrEmpty(intent.DataString))
            //     {
            //         var relativeUri = intent.Data?.Path + "?" + intent.Data?.Query;
            //         deeplinkService.OnAppLinkReceived(relativeUri);
            //     }
            // }
            //
            // base.OnCreate(savedInstanceState);
        }

        protected override void OnNewIntent(Intent intent)
        {
        
            var data = intent.DataString;
        
            if (intent.Action != Intent.ActionView) return;
            if (string.IsNullOrWhiteSpace(data)) return;
        
            var relativeUri = intent.Data.Path + "?" + intent.Data.Query;
            deeplinkService.OnAppLinkReceived(relativeUri);
            
            base.OnNewIntent(intent);

        }
    }
}