namespace App.Mobile.Android.Services.Local;

internal sealed class InMemoryMobileSelectedMediaStore :
    global::App.Mobile.Android.Services.Abstractions.IMobileSelectedMediaStore,
    IDisposable
{
    private readonly object _gate = new();
    private readonly SemaphoreSlim _snapshotLoadGate = new(1, 1);
    private readonly global::App.Mobile.Android.Services.Abstractions.IMobileSelectedMediaSnapshotStore _snapshotStore;
    private global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor? _currentDescriptor;
    private Func<Task<global::System.IO.Stream>>? _openCurrentReadFactory;
    private bool _snapshotLoaded;

    public InMemoryMobileSelectedMediaStore()
        : this(new NullMobileSelectedMediaSnapshotStore())
    {
    }

    public InMemoryMobileSelectedMediaStore(
        global::App.Mobile.Android.Services.Abstractions.IMobileSelectedMediaSnapshotStore snapshotStore)
    {
        _snapshotStore = snapshotStore;
    }

    public async Task<global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor?> GetCurrentAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await EnsureSnapshotLoadedAsync(cancellationToken);

        lock (_gate)
        {
            return _currentDescriptor;
        }
    }

    public async Task CacheAsync(
        global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor descriptor,
        Func<Task<global::System.IO.Stream>> openReadFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentNullException.ThrowIfNull(openReadFactory);
        await EnsureSnapshotLoadedAsync(cancellationToken);

        lock (_gate)
        {
            _currentDescriptor = descriptor;
            _openCurrentReadFactory = openReadFactory;
        }

        await _snapshotStore.SaveAsync(descriptor, cancellationToken);
    }

    public async Task<global::System.IO.Stream?> OpenCurrentReadAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await EnsureSnapshotLoadedAsync(cancellationToken);

        Func<Task<global::System.IO.Stream>>? openReadFactory;

        lock (_gate)
        {
            openReadFactory = _openCurrentReadFactory;
        }

        if (openReadFactory is null)
        {
            return null;
        }

        return await openReadFactory();
    }

    public async Task<global::App.Mobile.Android.Media.LocalSelectedMediaStoreEntry?> TakeCurrentAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await EnsureSnapshotLoadedAsync(cancellationToken);

        global::App.Mobile.Android.Media.LocalSelectedMediaStoreEntry? entry = null;

        lock (_gate)
        {
            if (_currentDescriptor is null)
            {
                return null;
            }

            if (_openCurrentReadFactory is null)
            {
                return null;
            }

            entry = new global::App.Mobile.Android.Media.LocalSelectedMediaStoreEntry(
                Descriptor: _currentDescriptor,
                OpenReadAsync: _openCurrentReadFactory);

            _currentDescriptor = null;
            _openCurrentReadFactory = null;
        }

        await _snapshotStore.SaveAsync(null, cancellationToken);
        return entry;
    }

    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await EnsureSnapshotLoadedAsync(cancellationToken);

        lock (_gate)
        {
            _currentDescriptor = null;
            _openCurrentReadFactory = null;
        }

        await _snapshotStore.SaveAsync(null, cancellationToken);
    }

    private async Task EnsureSnapshotLoadedAsync(CancellationToken cancellationToken)
    {
        if (_snapshotLoaded)
        {
            return;
        }

        await _snapshotLoadGate.WaitAsync(cancellationToken);
        try
        {
            if (_snapshotLoaded)
            {
                return;
            }

            var persistedDescriptor = await _snapshotStore.LoadAsync(cancellationToken);

            lock (_gate)
            {
                _currentDescriptor = persistedDescriptor is null
                    ? null
                    : persistedDescriptor with
                    {
                        HasLocalReadHandle = false
                    };
                _openCurrentReadFactory = null;
                _snapshotLoaded = true;
            }
        }
        finally
        {
            _snapshotLoadGate.Release();
        }
    }

    private sealed class NullMobileSelectedMediaSnapshotStore :
        global::App.Mobile.Android.Services.Abstractions.IMobileSelectedMediaSnapshotStore
    {
        public Task<global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor?> LoadAsync(
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor?>(null);
        }

        public Task SaveAsync(
            global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor? descriptor,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    public void Dispose()
    {
        _snapshotLoadGate.Dispose();
        GC.SuppressFinalize(this);
    }
}