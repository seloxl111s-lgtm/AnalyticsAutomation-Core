namespace App.Mobile.Android.Services.Local;

internal sealed class FileMobileSelectedMediaSnapshotStore :
    global::App.Mobile.Android.Services.Abstractions.IMobileSelectedMediaSnapshotStore
{
    private static readonly global::System.Text.Json.JsonSerializerOptions SerializerOptions = new();
    private readonly string _storageDirectory;

    public FileMobileSelectedMediaSnapshotStore(string storageDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(storageDirectory);

        _storageDirectory = storageDirectory;
        Directory.CreateDirectory(_storageDirectory);
    }

    public async Task<global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor?> LoadAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Directory.CreateDirectory(_storageDirectory);

        var snapshotPath = GetSnapshotPath();
        if (!File.Exists(snapshotPath))
        {
            return null;
        }

        await using var stream = File.OpenRead(snapshotPath);
        return await global::System.Text.Json.JsonSerializer.DeserializeAsync<
            global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor>(
            stream,
            SerializerOptions,
            cancellationToken);
    }

    public async Task SaveAsync(
        global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor? descriptor,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Directory.CreateDirectory(_storageDirectory);

        var snapshotPath = GetSnapshotPath();
        if (descriptor is null)
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
            descriptor,
            SerializerOptions,
            cancellationToken);
        await stream.FlushAsync(cancellationToken);
    }

    private string GetSnapshotPath()
    {
        return Path.Combine(_storageDirectory, "mobile-selected-media-snapshot.json");
    }
}