using Sfinx.Maui.Applink.Services;

namespace Sfinx.Maui.Applink;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }
    
    protected override async void OnAppLinkRequestReceived(Uri uri)
    {
        //base.OnAppLinkRequestReceived(uri);

        var deeplinkService =
            ServiceHelper.Current
                .GetRequiredService<DeeplinkAppService>();
        deeplinkService.OnAppLinkReceived(uri.ToString());
        // Show an alert to test the app link worked
        //
        // await this.Dispatcher.DispatchAsync(() =>
        // {he
        //     this.Windows[0].Page!.DisplayAlert(
        //         "App Link Opened",
        //         uri.ToString(),
        //         "OK");
        // });

        Console.WriteLine("APP LINK: " + uri.ToString());
    }

}