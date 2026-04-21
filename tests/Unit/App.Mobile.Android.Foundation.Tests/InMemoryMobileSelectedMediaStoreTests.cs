namespace App.Mobile.Android.Foundation.Tests;

public sealed class InMemoryMobileSelectedMediaStoreTests
{
    [Fact]
    public async Task CacheAsyncThenGetCurrentAsyncReturnsDescriptor()
    {
        var store = new global::App.Mobile.Android.Services.Local.InMemoryMobileSelectedMediaStore();
        var descriptor = CreateDescriptor();

        await store.CacheAsync(
            descriptor,
            () => Task.FromResult<global::System.IO.Stream>(
                new global::System.IO.MemoryStream([1, 2, 3])));

        var current = await store.GetCurrentAsync();

        Assert.Equal(descriptor, current);
    }

    [Fact]
    public async Task OpenCurrentReadAsyncReturnsReadableStreamAfterCache()
    {
        var store = new global::App.Mobile.Android.Services.Local.InMemoryMobileSelectedMediaStore();

        await store.CacheAsync(
            CreateDescriptor(),
            () => Task.FromResult<global::System.IO.Stream>(
                new global::System.IO.MemoryStream([5, 6, 7, 8])));

        await using var stream = await store.OpenCurrentReadAsync();

        Assert.NotNull(stream);
        Assert.True(stream!.CanRead);
        Assert.Equal(4, stream.Length);
    }

    [Fact]
    public async Task ClearAsyncClearsDescriptorAndReadHandle()
    {
        var store = new global::App.Mobile.Android.Services.Local.InMemoryMobileSelectedMediaStore();

        await store.CacheAsync(
            CreateDescriptor(),
            () => Task.FromResult<global::System.IO.Stream>(
                new global::System.IO.MemoryStream([9, 10])));

        await store.ClearAsync();

        var current = await store.GetCurrentAsync();
        var stream = await store.OpenCurrentReadAsync();

        Assert.Null(current);
        Assert.Null(stream);
    }

    private static global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor CreateDescriptor()
    {
        return new global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor(
            CacheKey: "cache-key-1",
            Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
            FileName: "sample.mp4",
            ContentType: "video/mp4",
            SelectedAtUtc: new DateTimeOffset(2026, 4, 21, 7, 0, 0, TimeSpan.Zero),
            HasLocalReadHandle: true);
    }
}
