namespace App.Mobile.Android.Foundation.Tests;

public sealed class StubMobileOutboxServiceTests
{
    [Fact]
    public async Task EnqueueStubItemAsyncCreatesOneItemWithQueuedStatus()
    {
        var service = CreateService();

        var result = await service.EnqueueStubItemAsync();
        var items = await service.GetItemsAsync();

        Assert.True(result.Applied);
        Assert.Single(items);
        Assert.Equal(global::App.Mobile.Android.Outbox.PendingSyncItemStatus.Queued, items[0].Status);
    }

    [Fact]
    public async Task RetryAsyncChangesStatusToRetryRequestedAndUpdatesLastActionText()
    {
        var service = CreateService();
        var enqueueResult = await service.EnqueueStubItemAsync();

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
        var service = CreateService();
        var enqueueResult = await service.EnqueueStubItemAsync();

        var removeResult = await service.RemoveAsync(enqueueResult.Item!.ItemId);
        var items = await service.GetItemsAsync();

        Assert.True(removeResult.Applied);
        Assert.Empty(items);
    }

    [Fact]
    public async Task RetryAsyncForUnknownItemReturnsAppliedFalse()
    {
        var service = CreateService();

        var result = await service.RetryAsync("missing-item");

        Assert.False(result.Applied);
        Assert.Null(result.Item);
    }

    [Fact]
    public async Task RemoveAsyncForUnknownItemReturnsAppliedFalse()
    {
        var service = CreateService();

        var result = await service.RemoveAsync("missing-item");

        Assert.False(result.Applied);
        Assert.Null(result.Item);
    }

    private static global::App.Mobile.Android.Services.Stubs.StubMobileOutboxService CreateService()
    {
        return new global::App.Mobile.Android.Services.Stubs.StubMobileOutboxService(
            global::Microsoft.Extensions.Logging.Abstractions.NullLogger<
                global::App.Mobile.Android.Services.Stubs.StubMobileOutboxService>.Instance);
    }
}
