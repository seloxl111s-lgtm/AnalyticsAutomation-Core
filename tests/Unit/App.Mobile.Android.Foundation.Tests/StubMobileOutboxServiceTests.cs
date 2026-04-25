namespace App.Mobile.Android.Foundation.Tests;

public sealed class StubMobileOutboxServiceTests
{
    [Fact]
    public async Task EnqueueCurrentSelectionAsyncWithoutCurrentSelectionReturnsAppliedFalse()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var context = CreateService(storageDirectory);

            var result = await context.Service.EnqueueCurrentSelectionAsync();

            Assert.False(result.Applied);
            Assert.Null(result.Item);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task EnqueueCurrentSelectionAsyncWithCachedSelectionCreatesOneQueuedItem()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var context = CreateService(storageDirectory);
            await CacheCurrentSelectionAsync(context.SelectedMediaStore);

            var result = await context.Service.EnqueueCurrentSelectionAsync();
            var items = await context.Service.GetItemsAsync();
            var currentSelection = await context.SelectedMediaStore.GetCurrentAsync();

            Assert.True(result.Applied);
            Assert.Single(items);
            Assert.Equal(global::App.Mobile.Android.Outbox.PendingSyncItemStatus.Queued, items[0].Status);
            Assert.NotNull(items[0].LocalMediaDraft);
            Assert.Equal("sample.mp4", items[0].LocalMediaDraft!.FileName);
            Assert.Null(currentSelection);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task PersistedQueueItemsLoadInNewServiceInstance()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var firstContext = CreateService(storageDirectory);
            await CacheCurrentSelectionAsync(firstContext.SelectedMediaStore);
            await firstContext.Service.EnqueueCurrentSelectionAsync();

            var secondContext = CreateService(storageDirectory);
            var items = await secondContext.Service.GetItemsAsync();

            Assert.Single(items);
            Assert.NotNull(items[0].LocalMediaDraft);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task LoadedLocalMediaDraftHasHasLocalReadHandleFalse()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var firstContext = CreateService(storageDirectory);
            await CacheCurrentSelectionAsync(firstContext.SelectedMediaStore);
            await firstContext.Service.EnqueueCurrentSelectionAsync();

            var secondContext = CreateService(storageDirectory);
            var items = await secondContext.Service.GetItemsAsync();

            Assert.Single(items);
            Assert.NotNull(items[0].LocalMediaDraft);
            Assert.False(items[0].LocalMediaDraft!.HasLocalReadHandle);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task EnqueueCurrentSelectionAsyncWithRestoredMetadataOnlyCurrentSelectionReturnsAppliedFalse()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var snapshotStore = new global::App.Mobile.Android.Services.Local.FileMobileSelectedMediaSnapshotStore(
                GetSelectedMediaStorageDirectory(storageDirectory));
            await snapshotStore.SaveAsync(CreateDescriptor("restored-cache-key"));

            var context = CreateService(storageDirectory);
            var result = await context.Service.EnqueueCurrentSelectionAsync();
            var currentSelection = await context.SelectedMediaStore.GetCurrentAsync();
            var items = await context.Service.GetItemsAsync();

            Assert.False(result.Applied);
            Assert.Null(result.Item);
            Assert.NotNull(currentSelection);
            Assert.False(currentSelection!.HasLocalReadHandle);
            Assert.Empty(items);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task EnqueueCurrentSelectionAsyncDuplicateCurrentSelectionReturnsAppliedFalse()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var context = CreateService(storageDirectory);
            await CacheCurrentSelectionAsync(context.SelectedMediaStore, cacheKey: "duplicate-cache-key");
            await context.Service.EnqueueCurrentSelectionAsync();
            await CacheCurrentSelectionAsync(context.SelectedMediaStore, cacheKey: "duplicate-cache-key");

            var result = await context.Service.EnqueueCurrentSelectionAsync();

            Assert.False(result.Applied);
            Assert.Null(result.Item);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task EnqueueCurrentSelectionAsyncDuplicateRejectionDoesNotClearCurrentSelectedMedia()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var context = CreateService(storageDirectory);
            await CacheCurrentSelectionAsync(context.SelectedMediaStore, cacheKey: "duplicate-cache-key");
            await context.Service.EnqueueCurrentSelectionAsync();
            await CacheCurrentSelectionAsync(context.SelectedMediaStore, cacheKey: "duplicate-cache-key");

            await context.Service.EnqueueCurrentSelectionAsync();
            var currentSelection = await context.SelectedMediaStore.GetCurrentAsync();

            Assert.NotNull(currentSelection);
            Assert.Equal("duplicate-cache-key", currentSelection!.CacheKey);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task EnqueueCurrentSelectionAsyncDuplicateRejectionDoesNotAddSecondQueueItem()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var context = CreateService(storageDirectory);
            await CacheCurrentSelectionAsync(context.SelectedMediaStore, cacheKey: "duplicate-cache-key");
            await context.Service.EnqueueCurrentSelectionAsync();
            await CacheCurrentSelectionAsync(context.SelectedMediaStore, cacheKey: "duplicate-cache-key");

            await context.Service.EnqueueCurrentSelectionAsync();
            var items = await context.Service.GetItemsAsync();

            Assert.Single(items);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task EnqueueStubItemAsyncStillWorksForCompatibility()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var context = CreateService(storageDirectory);
            var result = await context.Service.EnqueueStubItemAsync();
            var items = await context.Service.GetItemsAsync();

            Assert.True(result.Applied);
            Assert.Single(items);
            Assert.Null(items[0].LocalMediaDraft);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task EnqueueStubItemAsyncCreatesOneItemWithQueuedStatus()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var context = CreateService(storageDirectory);
            var result = await context.Service.EnqueueStubItemAsync();
            var items = await context.Service.GetItemsAsync();

            Assert.True(result.Applied);
            Assert.Single(items);
            Assert.Equal(global::App.Mobile.Android.Outbox.PendingSyncItemStatus.Queued, items[0].Status);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task RetryAsyncChangesStatusToRetryRequestedAndUpdatesLastActionText()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var context = CreateService(storageDirectory);
            await CacheCurrentSelectionAsync(context.SelectedMediaStore);
            var enqueueResult = await context.Service.EnqueueCurrentSelectionAsync();

            var retryResult = await context.Service.RetryAsync(enqueueResult.Item!.ItemId);
            var items = await context.Service.GetItemsAsync();

            Assert.True(retryResult.Applied);
            Assert.Single(items);
            Assert.Equal(global::App.Mobile.Android.Outbox.PendingSyncItemStatus.RetryRequested, items[0].Status);
            Assert.False(string.IsNullOrWhiteSpace(items[0].LastActionText));
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task RemoveAsyncRemovesTheItem()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var context = CreateService(storageDirectory);
            await CacheCurrentSelectionAsync(context.SelectedMediaStore);
            var enqueueResult = await context.Service.EnqueueCurrentSelectionAsync();

            var removeResult = await context.Service.RemoveAsync(enqueueResult.Item!.ItemId);
            var items = await context.Service.GetItemsAsync();

            Assert.True(removeResult.Applied);
            Assert.Empty(items);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task RemoveAsyncPersistsDeletion()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var firstContext = CreateService(storageDirectory);
            await CacheCurrentSelectionAsync(firstContext.SelectedMediaStore);
            var enqueueResult = await firstContext.Service.EnqueueCurrentSelectionAsync();
            await firstContext.Service.RemoveAsync(enqueueResult.Item!.ItemId);

            var secondContext = CreateService(storageDirectory);
            var items = await secondContext.Service.GetItemsAsync();

            Assert.Empty(items);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task RetryAsyncForUnknownItemReturnsAppliedFalse()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var context = CreateService(storageDirectory);
            var result = await context.Service.RetryAsync("missing-item");

            Assert.False(result.Applied);
            Assert.Null(result.Item);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    [Fact]
    public async Task RemoveAsyncForUnknownItemReturnsAppliedFalse()
    {
        var storageDirectory = CreateTempDirectory();

        try
        {
            var context = CreateService(storageDirectory);
            var result = await context.Service.RemoveAsync("missing-item");

            Assert.False(result.Applied);
            Assert.Null(result.Item);
        }
        finally
        {
            DeleteTempDirectory(storageDirectory);
        }
    }

    private static ServiceContext CreateService(string storageDirectory)
    {
        var selectedMediaSnapshotStore = new global::App.Mobile.Android.Services.Local.FileMobileSelectedMediaSnapshotStore(
            GetSelectedMediaStorageDirectory(storageDirectory));
        var outboxSnapshotStore = new global::App.Mobile.Android.Services.Local.FileMobileOutboxSnapshotStore(
            GetOutboxStorageDirectory(storageDirectory));
        var selectedMediaStore = new global::App.Mobile.Android.Services.Local.InMemoryMobileSelectedMediaStore(
            selectedMediaSnapshotStore);
        var duplicatePrecheckService = new global::App.Mobile.Android.Services.Local.LocalOutboxDuplicatePrecheckService();

        return new ServiceContext(
            new global::App.Mobile.Android.Services.Stubs.StubMobileOutboxService(
                duplicatePrecheckService,
                selectedMediaStore,
                outboxSnapshotStore,
                global::Microsoft.Extensions.Logging.Abstractions.NullLogger<
                    global::App.Mobile.Android.Services.Stubs.StubMobileOutboxService>.Instance),
            selectedMediaStore);
    }

    private static Task CacheCurrentSelectionAsync(
        global::App.Mobile.Android.Services.Local.InMemoryMobileSelectedMediaStore selectedMediaStore,
        string cacheKey = "selected-media-cache-key")
    {
        return selectedMediaStore.CacheAsync(
            CreateDescriptor(cacheKey),
            () => Task.FromResult<global::System.IO.Stream>(
                new global::System.IO.MemoryStream([1, 2, 3, 4])));
    }

    private static global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor CreateDescriptor(string cacheKey)
    {
        return new global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor(
            CacheKey: cacheKey,
            Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
            FileName: "sample.mp4",
            ContentType: "video/mp4",
            SelectedAtUtc: new DateTimeOffset(2026, 4, 21, 7, 0, 0, TimeSpan.Zero),
            HasLocalReadHandle: true);
    }

    private static string CreateTempDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), $"androida-mobile-outbox-{Guid.NewGuid():N}");
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

    private static string GetSelectedMediaStorageDirectory(string storageDirectory)
    {
        return Path.Combine(storageDirectory, "selected");
    }

    private static string GetOutboxStorageDirectory(string storageDirectory)
    {
        return Path.Combine(storageDirectory, "outbox");
    }

    private sealed record ServiceContext(
        global::App.Mobile.Android.Services.Stubs.StubMobileOutboxService Service,
        global::App.Mobile.Android.Services.Local.InMemoryMobileSelectedMediaStore SelectedMediaStore);
}