namespace App.Mobile.Android.Foundation.Tests;

public sealed class LocalOutboxDuplicatePrecheckServiceTests
{
    private readonly global::App.Mobile.Android.Services.Local.LocalOutboxDuplicatePrecheckService _service = new();

    [Fact]
    public void CheckAgainstOutboxNullSelectionReturnsCannotEnqueue()
    {
        var result = _service.CheckAgainstOutbox(null, []);

        Assert.False(result.CanEnqueue);
        Assert.Equal(
            global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckStatus.NoCurrentSelection,
            result.Status);
    }

    [Fact]
    public void CheckAgainstOutboxMatchingCacheKeyReturnsCannotEnqueue()
    {
        var result = _service.CheckAgainstOutbox(
            CreateSelection(cacheKey: "same-cache-key"),
            [CreateOutboxItem(cacheKey: "same-cache-key", fileName: "queued.mp4", contentType: "video/mp4")]);

        Assert.False(result.CanEnqueue);
        Assert.Equal(
            global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckStatus.LikelyAlreadyQueued,
            result.Status);
    }

    [Fact]
    public void CheckAgainstOutboxMatchingSourceFileNameAndContentTypeReturnsCannotEnqueue()
    {
        var result = _service.CheckAgainstOutbox(
            CreateSelection(cacheKey: "selection-cache-key", fileName: "VIDEO.MP4", contentType: "video/mp4"),
            [CreateOutboxItem(cacheKey: "queued-cache-key", fileName: "video.mp4", contentType: " VIDEO/MP4 ")]);

        Assert.False(result.CanEnqueue);
        Assert.Equal(
            global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckStatus.LikelyAlreadyQueued,
            result.Status);
    }

    [Fact]
    public void CheckAgainstOutboxDifferentQueuedItemReturnsCanEnqueue()
    {
        var result = _service.CheckAgainstOutbox(
            CreateSelection(cacheKey: "selection-cache-key", fileName: "fresh.mp4", contentType: "video/mp4"),
            [CreateOutboxItem(cacheKey: "queued-cache-key", fileName: "queued.mp4", contentType: "video/quicktime")]);

        Assert.True(result.CanEnqueue);
        Assert.Equal(
            global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckStatus.NoKnownDuplicateInOutbox,
            result.Status);
    }

    private static global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor CreateSelection(
        string cacheKey,
        string fileName = "sample.mp4",
        string? contentType = "video/mp4")
    {
        return new global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor(
            CacheKey: cacheKey,
            Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
            FileName: fileName,
            ContentType: contentType,
            SelectedAtUtc: new DateTimeOffset(2026, 4, 21, 7, 0, 0, TimeSpan.Zero),
            HasLocalReadHandle: true);
    }

    private static global::App.Mobile.Android.Outbox.PendingSyncItem CreateOutboxItem(
        string cacheKey,
        string fileName,
        string? contentType)
    {
        return new global::App.Mobile.Android.Outbox.PendingSyncItem(
            ItemId: "pending-sync-1",
            CreatedAtUtc: new DateTimeOffset(2026, 4, 21, 7, 5, 0, TimeSpan.Zero),
            Title: "Queue item",
            SummaryText: "Local queued item",
            Status: global::App.Mobile.Android.Outbox.PendingSyncItemStatus.Queued,
            LastActionText: "Queued",
            LocalMediaDraft: new global::App.Mobile.Android.Outbox.PendingSyncItemLocalMediaDraft(
                CacheKey: cacheKey,
                Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
                FileName: fileName,
                ContentType: contentType,
                SelectedAtUtc: new DateTimeOffset(2026, 4, 21, 7, 0, 0, TimeSpan.Zero),
                HasLocalReadHandle: true));
    }
}
