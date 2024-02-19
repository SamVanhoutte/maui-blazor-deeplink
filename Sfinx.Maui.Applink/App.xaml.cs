namespace Sfinx.Maui.Applink;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        //MainPage = new MainPage();
    }
    
    protected override async void OnAppLinkRequestReceived(Uri uri)
    {
        base.OnAppLinkRequestReceived(uri);

        // Show an alert to test the app link worked

        await this.Dispatcher.DispatchAsync(() =>
        {
            // At this point Windows collection is empty?!
            if (Windows.Any())
            {
                //Can we navigate here? 
                this.Windows[0].Page!.DisplayAlert(
                    $"App Link Opened {Windows.Count}",
                    uri.ToString(),
                    "OK");
                    
            }
        });

        Console.WriteLine("APP LINK: " + uri.ToString());
    }

    protected override Window CreateWindow(IActivationState? activationState)
        => new Window(new AppShell());
}