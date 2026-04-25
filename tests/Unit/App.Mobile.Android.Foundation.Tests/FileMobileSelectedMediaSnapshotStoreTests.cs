namespace App.Mobile.Android.Foundation.Tests;

public sealed class FileMobileSelectedMediaSnapshotStoreTests
{
    [Fact]
    public async Task SaveAsyncThenLoadAsyncReturnsDescriptor()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var store = new global::App.Mobile.Android.Services.Local.FileMobileSelectedMediaSnapshotStore(storageDirectory);
            var descriptor = CreateDescriptor();

            await store.SaveAsync(descriptor);
            var loaded = await store.LoadAsync();

            Assert.Equal(descriptor, loaded);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task SaveAsyncNullThenLoadAsyncReturnsNull()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var store = new global::App.Mobile.Android.Services.Local.FileMobileSelectedMediaSnapshotStore(storageDirectory);
            await store.SaveAsync(CreateDescriptor());

            await store.SaveAsync(null);
            var loaded = await store.LoadAsync();

            Assert.Null(loaded);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    private static global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor CreateDescriptor()
    {
        return new global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor(
            CacheKey: "selected-media-cache-key",
            Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
            FileName: "sample.mp4",
            ContentType: "video/mp4",
            SelectedAtUtc: new DateTimeOffset(2026, 4, 17, 10, 0, 0, TimeSpan.Zero),
            HasLocalReadHandle: true);
    }

    private static string CreateTempDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), $"androida-mobile-selected-{Guid.NewGuid():N}");
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