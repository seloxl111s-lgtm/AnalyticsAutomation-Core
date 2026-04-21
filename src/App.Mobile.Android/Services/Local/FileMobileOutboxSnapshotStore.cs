namespace App.Mobile.Android.Services.Local;

internal sealed class FileMobileOutboxSnapshotStore :
    global::App.Mobile.Android.Services.Abstractions.IMobileOutboxSnapshotStore
{
    private static readonly global::System.Text.Json.JsonSerializerOptions SerializerOptions = new();
    private readonly string _storageDirectory;

    public FileMobileOutboxSnapshotStore(string storageDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(storageDirectory);

        _storageDirectory = storageDirectory;
        Directory.CreateDirectory(_storageDirectory);
    }

    public async Task<IReadOnlyList<global::App.Mobile.Android.Outbox.PendingSyncItem>> LoadAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Directory.CreateDirectory(_storageDirectory);

        var snapshotPath = GetSnapshotPath();
        if (!File.Exists(snapshotPath))
        {
            return [];
        }

        await using var stream = File.OpenRead(snapshotPath);
        var items = await global::System.Text.Json.JsonSerializer.DeserializeAsync<
            List<global::App.Mobile.Android.Outbox.PendingSyncItem>>(
            stream,
            SerializerOptions,
            cancellationToken);

        return items ?? [];
    }

    public async Task SaveAsync(
        IReadOnlyList<global::App.Mobile.Android.Outbox.PendingSyncItem> items,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(items);
        Directory.CreateDirectory(_storageDirectory);

        var snapshotPath = GetSnapshotPath();
        if (items.Count == 0)
        {
            if (File.Exists(snapshotPath))
            {
                File.Delete(snapshotPath);
            }

            return;
        }

        await using var stream = File.Create(snapshotPath);
        await global::System.Text.Json.JsonSerializer.SerializeAsync(
            stream,
            items.ToArray(),
            SerializerOptions,
            cancellationToken);
        await stream.FlushAsync(cancellationToken);
    }

    private string GetSnapshotPath()
    {
        return Path.Combine(_storageDirectory, "mobile-outbox-snapshot.json");
    }
}
