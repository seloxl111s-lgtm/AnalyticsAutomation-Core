namespace App.Mobile.Android.Services.Abstractions;

internal interface IFeatureFlagReader
{
    bool IsEnabled(string flagName);
}
