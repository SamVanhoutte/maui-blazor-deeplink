using System.Diagnostics;

namespace Sfinx.Maui.Applink;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        //MainPage = new MainPage();
    }
    
    // protected override async void OnAppLinkRequestReceived(Uri uri)
    // {
    //     base.OnAppLinkRequestReceived(uri);
    //
    //     // Show an alert to test the app link worked
    //     //         Preferences.Set("requestedUrl", url);
    //
    //     Trace.WriteLine("On app link requested pushing modal");
    //     // await this.Dispatcher.DispatchAsync(() => 
    //     //     App.Current.MainPage.Navigation.PushModalAsync(new Applink.MainPage()));
    //
    //     Console.WriteLine("APP LINK: " + uri.ToString());
    // }

    protected override Window CreateWindow(IActivationState? activationState)
        => new Window(new AppShell());
}