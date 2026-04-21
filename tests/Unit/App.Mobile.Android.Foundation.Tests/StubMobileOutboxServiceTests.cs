namespace App.Mobile.Android.Foundation.Tests;

public sealed class StubMobileOutboxServiceTests
{
    [Fact]
    public async Task EnqueueCurrentSelectionAsyncWithoutCurrentSelectionReturnsAppliedFalse()
    {
        var (service, _) = CreateService();

        var result = await service.EnqueueCurrentSelectionAsync();

        Assert.False(result.Applied);
        Assert.Null(result.Item);
    }

    [Fact]
    public async Task EnqueueCurrentSelectionAsyncWithCachedSelectionCreatesOneQueuedItem()
    {
        var (service, selectedMediaStore) = CreateService();
        await CacheCurrentSelectionAsync(selectedMediaStore);

        var result = await service.EnqueueCurrentSelectionAsync();
        var items = await service.GetItemsAsync();
        var currentSelection = await selectedMediaStore.GetCurrentAsync();

        Assert.True(result.Applied);
        Assert.Single(items);
        Assert.Equal(global::App.Mobile.Android.Outbox.PendingSyncItemStatus.Queued, items[0].Status);
        Assert.NotNull(items[0].LocalMediaDraft);
        Assert.Equal("sample.mp4", items[0].LocalMediaDraft!.FileName);
        Assert.Null(currentSelection);
    }

    [Fact]
    public async Task EnqueueStubItemAsyncStillWorksForCompatibility()
    {
        var (service, _) = CreateService();

        var result = await service.EnqueueStubItemAsync();
        var items = await service.GetItemsAsync();

        Assert.True(result.Applied);
        Assert.Single(items);
        Assert.Null(items[0].LocalMediaDraft);
    }

    [Fact]
    public async Task EnqueueStubItemAsyncCreatesOneItemWithQueuedStatus()
    {
        var (service, _) = CreateService();

        var result = await service.EnqueueStubItemAsync();
        var items = await service.GetItemsAsync();

        Assert.True(result.Applied);
        Assert.Single(items);
        Assert.Equal(global::App.Mobile.Android.Outbox.PendingSyncItemStatus.Queued, items[0].Status);
    }

    [Fact]
    public async Task RetryAsyncChangesStatusToRetryRequestedAndUpdatesLastActionText()
    {
        var (service, selectedMediaStore) = CreateService();
        await CacheCurrentSelectionAsync(selectedMediaStore);
        var enqueueResult = await service.EnqueueCurrentSelectionAsync();

        var retryResult = await service.RetryAsync(enqueueResult.Item!.ItemId);
        var items = await service.GetItemsAsync();

        Assert.True(retryResult.Applied);
        Assert.Single(items);
        Assert.Equal(global::App.Mobile.Android.Outbox.PendingSyncItemStatus.RetryRequested, items[0].Status);
        Assert.False(string.IsNullOrWhiteSpace(items[0].LastActionText));
    }

    [Fact]
    public async Task RemoveAsyncRemovesTheItem()
    {
        var (service, selectedMediaStore) = CreateService();
        await CacheCurrentSelectionAsync(selectedMediaStore);
        var enqueueResult = await service.EnqueueCurrentSelectionAsync();

        var removeResult = await service.RemoveAsync(enqueueResult.Item!.ItemId);
        var items = await service.GetItemsAsync();

        Assert.True(removeResult.Applied);
        Assert.Empty(items);
    }

    [Fact]
    public async Task RetryAsyncForUnknownItemReturnsAppliedFalse()
    {
        var (service, _) = CreateService();

        var result = await service.RetryAsync("missing-item");

        Assert.False(result.Applied);
        Assert.Null(result.Item);
    }

    [Fact]
    public async Task RemoveAsyncForUnknownItemReturnsAppliedFalse()
    {
        var (service, _) = CreateService();

        var result = await service.RemoveAsync("missing-item");

        Assert.False(result.Applied);
        Assert.Null(result.Item);
    }

    private static (
        global::App.Mobile.Android.Services.Stubs.StubMobileOutboxService Service,
        global::App.Mobile.Android.Services.Local.InMemoryMobileSelectedMediaStore SelectedMediaStore) CreateService()
    {
        var selectedMediaStore = new global::App.Mobile.Android.Services.Local.InMemoryMobileSelectedMediaStore();

        return (
            new global::App.Mobile.Android.Services.Stubs.StubMobileOutboxService(
                selectedMediaStore,
                global::Microsoft.Extensions.Logging.Abstractions.NullLogger<
                    global::App.Mobile.Android.Services.Stubs.StubMobileOutboxService>.Instance),
            selectedMediaStore);
    }

    private static Task CacheCurrentSelectionAsync(
        global::App.Mobile.Android.Services.Local.InMemoryMobileSelectedMediaStore selectedMediaStore)
    {
        return selectedMediaStore.CacheAsync(
            new global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor(
                CacheKey: "selected-media-cache-key",
                Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
                FileName: "sample.mp4",
                ContentType: "video/mp4",
                SelectedAtUtc: new DateTimeOffset(2026, 4, 21, 7, 0, 0, TimeSpan.Zero),
                HasLocalReadHandle: true),
            () => Task.FromResult<global::System.IO.Stream>(
                new global::System.IO.MemoryStream([1, 2, 3, 4])));
    }
}