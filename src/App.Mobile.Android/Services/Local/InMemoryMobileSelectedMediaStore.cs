namespace App.Mobile.Android.Services.Local;

internal sealed class InMemoryMobileSelectedMediaStore :
    global::App.Mobile.Android.Services.Abstractions.IMobileSelectedMediaStore
{
    private readonly object _gate = new();
    private global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor? _currentDescriptor;
    private Func<Task<global::System.IO.Stream>>? _openCurrentReadFactory;

    public Task<global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor?> GetCurrentAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        lock (_gate)
        {
            return Task.FromResult(_currentDescriptor);
        }
    }

    public Task CacheAsync(
        global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor descriptor,
        Func<Task<global::System.IO.Stream>> openReadFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentNullException.ThrowIfNull(openReadFactory);

        lock (_gate)
        {
            _currentDescriptor = descriptor;
            _openCurrentReadFactory = openReadFactory;
        }

        return Task.CompletedTask;
    }

    public Task<global::System.IO.Stream?> OpenCurrentReadAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Func<Task<global::System.IO.Stream>>? openReadFactory;

        lock (_gate)
        {
            openReadFactory = _openCurrentReadFactory;
        }

        if (openReadFactory is null)
        {
            return Task.FromResult<global::System.IO.Stream?>(null);
        }

        return OpenCurrentReadCoreAsync(openReadFactory);
    }

    public Task ClearAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        lock (_gate)
        {
            _currentDescriptor = null;
            _openCurrentReadFactory = null;
        }

        return Task.CompletedTask;
    }

    private static async Task<global::System.IO.Stream?> OpenCurrentReadCoreAsync(
        Func<Task<global::System.IO.Stream>> openReadFactory)
    {
        return await openReadFactory();
    }
}
