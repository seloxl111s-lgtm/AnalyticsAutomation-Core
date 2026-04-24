namespace App.Mobile.Android.Services.Stubs;

internal sealed class StubFeatureFlagReader :
    global::App.Mobile.Android.Services.Abstractions.IFeatureFlagReader
{
    public const string ShellBannerFlag = "mobile.shell.banner";
    public const string QueueFoundationCardFlag = "mobile.queue.foundation-card";

    public bool IsEnabled(string flagName)
    {
        return string.Equals(flagName, ShellBannerFlag, global::System.StringComparison.Ordinal)
            || string.Equals(flagName, QueueFoundationCardFlag, global::System.StringComparison.Ordinal);
    }
}