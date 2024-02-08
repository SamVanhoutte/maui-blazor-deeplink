using System.Diagnostics;
using Android.Content;
using Microsoft.AspNetCore.Components.WebView;
using Sfinx.Maui.Applink.Services;

namespace Sfinx.Maui.Applink;

public partial class MainPage : ContentPage, IDisposable
{
    //private DeeplinkAppService deeplinkAppService;
    
    public MainPage()
    {
        InitializeComponent();
        //APPLINK
        // blazorWebView.UrlLoading += (sender, urlLoadingEventArgs) =>
        // {
        //     Console.WriteLine($"Loading url {urlLoadingEventArgs.Url}");
        //     if (urlLoadingEventArgs.Url.Host != "0.0.0.0")
        //     {
        //         urlLoadingEventArgs.UrlLoadingStrategy = UrlLoadingStrategy.OpenInWebView;
        //     }
        // };

        // this.deeplinkAppService = deeplinkAppService;
        // if (!DeeplinkAppService.IsRegistered)
        // {
        //     Console.WriteLine("on init");
        //     this.deeplinkAppService.AppLinkReceived += AppServices_AppLinkReceived;
        //     this.deeplinkAppService.RegisterListener();
        //     if (!string.IsNullOrEmpty(deeplinkAppService.LastAppLink))
        //     {
        //         AppServices_AppLinkReceived(null, new() { Data = this.deeplinkAppService.LastAppLink });
        //     }
        // }

    }

    private async Task NavigateTo(string blazorRoute)
    {
        
    }
    
    private void AppServices_AppLinkReceived(object? sender, AppLinkReceivedEventArgs e)
    {
        Trace.WriteLine($"Will navigate now! {e.Data}");
        var url = e.Data;
        //deeplinkAppService.ResetLastAppLink();
        //NavigationManager.NavigateTo(e.Data, forceLoad: true);
    }

    void IDisposable.Dispose()
    {
        // deeplinkAppService.AppLinkReceived -= AppServices_AppLinkReceived;
        // DeeplinkAppService.IsRegistered = false;
    }
}