namespace App.Mobile.Android.Services.Abstractions;

internal interface IMobileSelectedMediaStore
{
    Task<global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor?> GetCurrentAsync(
        CancellationToken cancellationToken = default);

    Task CacheAsync(
        global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor descriptor,
        Func<Task<global::System.IO.Stream>> openReadFactory,
        CancellationToken cancellationToken = default);

    Task<global::System.IO.Stream?> OpenCurrentReadAsync(
        CancellationToken cancellationToken = default);

    Task ClearAsync(
        CancellationToken cancellationToken = default);
}