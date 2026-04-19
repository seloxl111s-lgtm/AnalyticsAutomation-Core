namespace App.Mobile.Android.Services.Abstractions;

internal interface IMobileAccessContext
{
    global::App.Mobile.Android.State.MobileShellMode CurrentMode { get; }

    bool CanAccess(global::App.Mobile.Android.Navigation.MobileViewId viewId);

    string GetBannerText();
}
