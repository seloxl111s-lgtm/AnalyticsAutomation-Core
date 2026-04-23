namespace App.Mobile.Android.Options;

internal sealed class MobileShellOptions
{
    public global::App.Mobile.Android.State.MobileShellMode DefaultMode { get; set; } =
        global::App.Mobile.Android.State.MobileShellMode.Development;

    public bool ShowShellBanner { get; set; } = true;
}