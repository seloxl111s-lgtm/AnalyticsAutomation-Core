namespace App.Mobile.Android.Foundation.Tests;

public sealed class FileMobileOutboxSnapshotStoreTests
{
    [Fact]
    public async Task SaveAsyncThenLoadAsyncReturnsItems()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var store = new global::App.Mobile.Android.Services.Local.FileMobileOutboxSnapshotStore(storageDirectory);
            var items = CreateItems();

            await store.SaveAsync(items);
            var loaded = await store.LoadAsync();

            Assert.Equal(items, loaded);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task SaveAsyncEmptyListThenLoadAsyncReturnsEmpty()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var store = new global::App.Mobile.Android.Services.Local.FileMobileOutboxSnapshotStore(storageDirectory);
            await store.SaveAsync(CreateItems());

            await store.SaveAsync([]);
            var loaded = await store.LoadAsync();

            Assert.Empty(loaded);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    private static IReadOnlyList<global::App.Mobile.Android.Outbox.PendingSyncItem> CreateItems()
    {
        return
        [
            new global::App.Mobile.Android.Outbox.PendingSyncItem(
                ItemId: "pending-sync-1",
                CreatedAtUtc: new DateTimeOffset(2026, 4, 17, 10, 5, 0, TimeSpan.Zero),
                Title: "Draft 1",
                SummaryText: "Local item",
                Status: global::App.Mobile.Android.Outbox.PendingSyncItemStatus.Queued,
                LastActionText: "Queued",
                LocalMediaDraft: new global::App.Mobile.Android.Outbox.PendingSyncItemLocalMediaDraft(
                    CacheKey: "cache-key-1",
                    Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
                    FileName: "video-1.mp4",
                    ContentType: "video/mp4",
                    SelectedAtUtc: new DateTimeOffset(2026, 4, 17, 10, 0, 0, TimeSpan.Zero),
                    HasLocalReadHandle: true))
        ];
    }

    private static string CreateTempDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), $"androida-mobile-outbox-snapshot-{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        return path;
    }

    private static void DeleteTempDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, recursive: true);
        }
    }
}
