# MAUI 8 AppLink with Blazor repro
Repro sample for github issue on MAUI Android deep linking

[Github issue](https://github.com/dotnet/maui/issues/20267)

## Sample links to test the app from browser

[Lock 01](sfinx://app/locks/lock-01)

[Lock 02](sfinx://app/locks/lock-02)

## Actual issue description

### Description

Hello, 

This is quite a specific case that is making us loose credibility at customers, where the app we've built is being used for delivery of goods.  

We have built a MAUI app and have seen this specific behavior both on net7 as well as net8.  
Another app is linking to a specific location in our app, so we can open (in our **BlazorView**) the right information.  They are just using the link in the following format: `sfinx://app/locks/{lockid}`.  

That link is opening the app in the correct location and everything works fine (see code below).  
However, the specific issue is when the following sequence occurs:

1. User clicks link in the other app (or in a browser)
2. App opens successfully on the right location
3. **User clicks the Android back button**
4. Android navigates back to the initial app (or browser)
5. When a link is clicked again from within this app or browser, the Android MAUI app crashes and hangs, with no other possibility than to close the app (sometimes a few times). 


**Findings**

- When the back button is not being used in this sequence, things keep working!
- On iOS this seems to be working

**Code**

`MainActivity.cs`

```csharp
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Identity.Client;
using Sfinx.ClientApp.Services.DeepLink;

namespace Sfinx.ClientApp
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
            // MAUI cross-platform service resolver: https://stackoverflow.com/a/73521158/10388359
            deeplinkService = ServiceHelper.Current.GetRequiredService<DeeplinkAppService>(); 
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            OnNewIntent(this.Intent);
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
```

`DeeplinkAppService.cs`, this service is injected as a Singleton and just 'publishes' the event of the `AppLinkReceived` to where its used (see last snippet)

```csharp
public class DeeplinkAppService
{
    public event EventHandler<AppLinkReceivedEventArgs>? AppLinkReceived;
    public string? LastAppLink { get; private set; }

    public void OnAppLinkReceived(string data)
    {
        LastAppLink = data;

        AppLinkReceived?.Invoke(this, new() { Data = data });
    }

    public void ResetLastAppLink()
    {
        LastAppLink = null;
    }
}
```

Using the NavigationManager and DeepLinkService in the `Main.razor` page

```csharp
@using Sfinx.App.Shared
@using Sfinx.ClientApp.Services.DeepLink
@implements IDisposable
@inject DeeplinkAppService deeplinkAppService 
<Router AppAssembly="@typeof(Main).Assembly" AdditionalAssemblies="@(new[] { typeof(SharedResources).Assembly })">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)"/>
        <FocusOnNavigate RouteData="@routeData" Selector="h1"/>
    </Found>
    <NotFound>
        <LayoutView Layout="@typeof(MainLayout)">
            <p>Sorry, there's nothing at this address.</p>
        </LayoutView>
    </NotFound>
</Router>

@code
{
    [Inject] public NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        if (!DeeplinkAppService.IsRegistered)
        {
            Console.WriteLine("on init");
            deeplinkAppService.AppLinkReceived += AppServices_AppLinkReceived;
            deeplinkAppService.RegisterListener();
            if (!string.IsNullOrEmpty(deeplinkAppService.LastAppLink))
            {
                AppServices_AppLinkReceived(null, new() { Data = deeplinkAppService.LastAppLink });
            }
        }

        base.OnInitialized();
    }

    private void AppServices_AppLinkReceived(object? sender, AppLinkReceivedEventArgs e)
    {
        var url = e.Data;
        deeplinkAppService.ResetLastAppLink();
        NavigationManager.NavigateTo(e.Data, forceLoad: true);
    }

    void IDisposable.Dispose()
    {
        deeplinkAppService.AppLinkReceived -= AppServices_AppLinkReceived;
        DeeplinkAppService.IsRegistered = false;
    }
}
```

### Steps to Reproduce

1. Compile an app with `BlazorWebView` for Android
2. Configure the `MainActivity` as shown above, and express the intents in the manifest file
3. Inject the `DeepLinkService` as a Singleton 
4. Update the `Main.razor` as shown above, so that the `NavigationManger` can navigate, when the event is triggered
5. Run the app (can be on emulator)
6. Trigger the app from a browser `<a href="sfinx://app/locks/dkdk" />`
7. See that the correct page in Blazor is opened (url Locks/dkdk)
8. Click the Android back button and see you get back to the browser
9. Click the link again
10. Notice the app hangs
