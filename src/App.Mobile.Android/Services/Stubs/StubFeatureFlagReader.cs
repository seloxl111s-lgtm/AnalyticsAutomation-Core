namespace App.Mobile.Android.Services.Stubs;

internal sealed class StubFeatureFlagReader :
    global::App.Mobile.Android.Services.Abstractions.IFeatureFlagReader
{
    public const string ShellBannerFlag = "mobile.shell.banner";

    public bool IsEnabled(string flagName)
    {
        return string.Equals(flagName, ShellBannerFlag, global::System.StringComparison.Ordinal);
    }
}
