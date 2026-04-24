namespace App.Mobile.Android.Services.Abstractions;

internal interface IMobileMediaService
{
    Task<global::App.Mobile.Android.Media.MobileMediaCapabilitySnapshot> GetCapabilitySnapshotAsync(
        CancellationToken cancellationToken = default);

    Task<global::App.Mobile.Android.Media.MobileMediaOperationResult> PickVideoAsync(
        CancellationToken cancellationToken = default);

    Task<global::App.Mobile.Android.Media.MobileMediaOperationResult> CaptureVideoAsync(
        CancellationToken cancellationToken = default);
}