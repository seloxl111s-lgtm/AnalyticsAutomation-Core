namespace App.Mobile.Android.Foundation.Tests;

public sealed class LocalCurrentSelectionDraftRepairServiceTests
{
    private readonly global::App.Mobile.Android.Services.Local.LocalCurrentSelectionDraftRepairService _service = new();

    [Fact]
    public void CheckCurrentSelectionForDraftRepair_NullSelection_ReturnsCannotApply()
    {
        var result = _service.CheckCurrentSelectionForDraftRepair(
            null,
            CreateRepairableItem());

        Assert.False(result.CanApply);
        Assert.Equal(
            global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.NoCurrentSelection,
            result.Status);
    }

    [Fact]
    public void CheckCurrentSelectionForDraftRepair_SelectionWithoutLiveHandle_ReturnsCannotApply()
    {
        var result = _service.CheckCurrentSelectionForDraftRepair(
            CreateSelection(hasLocalReadHandle: false),
            CreateRepairableItem());

        Assert.False(result.CanApply);
        Assert.Equal(
            global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.SelectionHasNoLiveHandle,
            result.Status);
    }

    [Fact]
    public void CheckCurrentSelectionForDraftRepair_TargetWithoutLocalMediaDraft_ReturnsCannotApply()
    {
        var result = _service.CheckCurrentSelectionForDraftRepair(
            CreateSelection(),
            CreateItemWithoutDraft());

        Assert.False(result.CanApply);
        Assert.Equal(
            global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.NoRepairableDraft,
            result.Status);
    }

    [Fact]
    public void CheckCurrentSelectionForDraftRepair_AlreadyRepairedDraft_ReturnsCannotApply()
    {
        var result = _service.CheckCurrentSelectionForDraftRepair(
            CreateSelection(),
            CreateRepairableItem(hasLocalReadHandle: true));

        Assert.False(result.CanApply);
        Assert.Equal(
            global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.DraftAlreadyHasLiveHandle,
            result.Status);
    }

    [Fact]
    public void CheckCurrentSelectionForDraftRepair_MatchingCacheKey_ReturnsCanApply()
    {
        var result = _service.CheckCurrentSelectionForDraftRepair(
            CreateSelection(cacheKey: "same-cache-key"),
            CreateRepairableItem(cacheKey: "same-cache-key"));

        Assert.True(result.CanApply);
        Assert.Equal(
            global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.ReadyToRepair,
            result.Status);
    }

    [Fact]
    public void CheckCurrentSelectionForDraftRepair_MatchingFileNameAndContentType_ReturnsCanApply()
    {
        var result = _service.CheckCurrentSelectionForDraftRepair(
            CreateSelection(cacheKey: "selection-cache-key", fileName: "VIDEO.MP4", contentType: "video/mp4"),
            CreateRepairableItem(cacheKey: "draft-cache-key", fileName: "video.mp4", contentType: " VIDEO/MP4 "));

        Assert.True(result.CanApply);
        Assert.Equal(
            global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.ReadyToRepair,
            result.Status);
    }

    [Fact]
    public void CheckCurrentSelectionForDraftRepair_MismatchedSelection_ReturnsCannotApply()
    {
        var result = _service.CheckCurrentSelectionForDraftRepair(
            CreateSelection(fileName: "fresh.mp4", contentType: "video/mp4"),
            CreateRepairableItem(fileName: "queued.mov", contentType: "video/quicktime"));

        Assert.False(result.CanApply);
        Assert.Equal(
            global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.SelectionDoesNotMatchDraft,
            result.Status);
    }

    private static global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor CreateSelection(
        string cacheKey = "selected-cache-key",
        string fileName = "sample.mp4",
        string? contentType = "video/mp4",
        bool hasLocalReadHandle = true)
    {
        return new global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor(
            CacheKey: cacheKey,
            Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
            FileName: fileName,
            ContentType: contentType,
            SelectedAtUtc: new DateTimeOffset(2026, 4, 17, 12, 0, 0, TimeSpan.Zero),
            HasLocalReadHandle: hasLocalReadHandle);
    }

    private static global::App.Mobile.Android.Outbox.PendingSyncItem CreateItemWithoutDraft()
    {
        return new global::App.Mobile.Android.Outbox.PendingSyncItem(
            ItemId: "pending-sync-no-draft",
            CreatedAtUtc: new DateTimeOffset(2026, 4, 17, 12, 5, 0, TimeSpan.Zero),
            Title: "Draft without media",
            SummaryText: "Local draft",
            Status: global::App.Mobile.Android.Outbox.PendingSyncItemStatus.Queued,
            LastActionText: null,
            LocalMediaDraft: null);
    }

    private static global::App.Mobile.Android.Outbox.PendingSyncItem CreateRepairableItem(
        string cacheKey = "draft-cache-key",
        string fileName = "sample.mp4",
        string? contentType = "video/mp4",
        bool hasLocalReadHandle = false)
    {
        return new global::App.Mobile.Android.Outbox.PendingSyncItem(
            ItemId: "pending-sync-1",
            CreatedAtUtc: new DateTimeOffset(2026, 4, 17, 12, 5, 0, TimeSpan.Zero),
            Title: "Draft 1",
            SummaryText: "Local draft",
            Status: global::App.Mobile.Android.Outbox.PendingSyncItemStatus.Queued,
            LastActionText: global::App.Mobile.Android.Localization.MobileUiText.PendingSyncRestoredMetadataLastActionText,
            LocalMediaDraft: new global::App.Mobile.Android.Outbox.PendingSyncItemLocalMediaDraft(
                CacheKey: cacheKey,
                Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
                FileName: fileName,
                ContentType: contentType,
                SelectedAtUtc: new DateTimeOffset(2026, 4, 17, 12, 0, 0, TimeSpan.Zero),
                HasLocalReadHandle: hasLocalReadHandle));
    }
}