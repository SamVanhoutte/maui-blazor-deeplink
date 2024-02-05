using Microsoft.AspNetCore.Components;

namespace Sfinx.Maui.Applink.Services;

public class DeeplinkAppService
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    public event EventHandler<AppLinkReceivedEventArgs>? AppLinkReceived;
    public string? LastAppLink { get; private set; }

    public void OnAppLinkReceived(string data)
    {
        LastAppLink = data;

        NavigationManager.NavigateTo(data);
        //AppLinkReceived?.Invoke(this, new() { Data = data });
    }

    public void ResetLastAppLink()
    {
        LastAppLink = null;
    }

    public void RegisterListener()
    {
        IsRegistered = true;
    }

    public static bool IsRegistered { get; set; }
}

//
// public static class DeepLinkService
// {
//     public delegate void PageSetEventHandler(object sender, object e);
//
//     public static event PageSetEventHandler PageSet;
//
//     public static string Page { get; private set; }
//
//     public static void SetPage(string url)
//     {
//         Page = url;
//         if (!string.IsNullOrEmpty(url))
//         {
//             PageSet?.Invoke(null, url);
//         }
//     }
//}