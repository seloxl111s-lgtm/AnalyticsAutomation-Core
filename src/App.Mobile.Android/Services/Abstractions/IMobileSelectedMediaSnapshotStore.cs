namespace App.Mobile.Android.Services.Abstractions;

internal interface IMobileSelectedMediaSnapshotStore
{
    Task<global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor?> LoadAsync(
        CancellationToken cancellationToken = default);

    Task SaveAsync(
        global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor? descriptor,
        CancellationToken cancellationToken = default);
}