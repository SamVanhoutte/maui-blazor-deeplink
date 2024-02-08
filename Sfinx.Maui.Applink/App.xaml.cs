using System.Diagnostics;
using Sfinx.Maui.Applink.Services;

namespace Sfinx.Maui.Applink;

public partial class App : Application
{
    // private readonly DeeplinkAppService deeplinkService;

    
    public App()
    {
        InitializeComponent();

        // deeplinkService =
        //     ServiceHelper.Current
        //         .GetRequiredService<DeeplinkAppService>(); 

        // APPLINK
        // MainPage = new MainPage(deeplinkService);
        MainPage = new NavigationPage( new MainPage());
        
    }
    //
    // protected override async void OnAppLinkRequestReceived(Uri uri)
    // {
    //     base.OnAppLinkRequestReceived(uri);
    //
    //     // Show an alert to test the app link worked
    //     Trace.WriteLine($"Handling uri in app now : {uri}");
    //     await this.Dispatcher.DispatchAsync(() =>
    //     {
    //         deeplinkService.OnAppLinkReceived(uri.ToString());
    //     });
    //     Trace.WriteLine($"App service received : {uri}");
    //     // await this.Dispatcher.DispatchAsync(() =>
    //     // {
    //     //     this.Windows[0].Page!.DisplayAlert(
    //     //         "App Link Opened",
    //     //         uri.ToString(),
    //     //         "OK");
    //     // });
    //
    // }
}