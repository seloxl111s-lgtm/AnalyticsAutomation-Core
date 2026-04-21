namespace App.Mobile.Android.Foundation.Tests;

public sealed class InMemoryMobileReportDraftStoreTests
{
    [Fact]
    public async Task CreateFpvDraftAsync_CreatesOneDraftWithDraftStatus()
    {
        var store = CreateStore();

        var draft = await store.CreateFpvDraftAsync();

        Assert.Equal(global::App.Mobile.Android.Reports.MobileReportDraftStatus.Draft, draft.Status);
        Assert.NotEmpty(draft.DraftId);
    }

    [Fact]
    public async Task CreatedDraft_ContainsObservedPlaceholderFieldLabels()
    {
        var store = CreateStore();

        var draft = await store.CreateFpvDraftAsync();
        var labels = draft.Fields.Select(field => field.Label).ToArray();

        Assert.Contains(global::App.Mobile.Android.Localization.MobileUiText.ReportFieldDeviceTypeLabel, labels);
        Assert.Contains(global::App.Mobile.Android.Localization.MobileUiText.ReportFieldSerialNumberLabel, labels);
        Assert.Contains(global::App.Mobile.Android.Localization.MobileUiText.ReportFieldDeliveryStartLabel, labels);
        Assert.Contains(global::App.Mobile.Android.Localization.MobileUiText.ReportFieldDeliveryTimeLabel, labels);
        Assert.Contains(global::App.Mobile.Android.Localization.MobileUiText.ReportFieldDistanceLabel, labels);
        Assert.Contains(global::App.Mobile.Android.Localization.MobileUiText.ReportFieldTargetTypeLabel, labels);
        Assert.Contains(global::App.Mobile.Android.Localization.MobileUiText.ReportFieldReasonLabel, labels);
        Assert.Contains(global::App.Mobile.Android.Localization.MobileUiText.ReportFieldCommentLabel, labels);
        Assert.Contains(global::App.Mobile.Android.Localization.MobileUiText.ReportFieldRadioFrequencyLabel, labels);
        Assert.Contains(global::App.Mobile.Android.Localization.MobileUiText.ReportFieldVideoFrequencyLabel, labels);
        Assert.Contains(global::App.Mobile.Android.Localization.MobileUiText.ReportFieldTestFlightLabel, labels);
    }

    [Fact]
    public async Task GetDraftsAsync_ReturnsCreatedDraft()
    {
        var store = CreateStore();
        var createdDraft = await store.CreateFpvDraftAsync();

        var drafts = await store.GetDraftsAsync();

        Assert.Single(drafts);
        Assert.Equal(createdDraft.DraftId, drafts[0].DraftId);
    }

    [Fact]
    public async Task GetDraftAsync_ReturnsCreatedDraft()
    {
        var store = CreateStore();
        var createdDraft = await store.CreateFpvDraftAsync();

        var draft = await store.GetDraftAsync(createdDraft.DraftId);

        Assert.NotNull(draft);
        Assert.Equal(createdDraft.DraftId, draft!.DraftId);
    }

    [Fact]
    public async Task AttachSelectedVideoAsync_AddsVideoAttachmentMetadata()
    {
        var store = CreateStore();
        var draft = await store.CreateFpvDraftAsync();
        var descriptor = CreateDescriptor();

        var updatedDraft = await store.AttachSelectedVideoAsync(draft.DraftId, descriptor);

        Assert.NotNull(updatedDraft);
        Assert.Equal(global::App.Mobile.Android.Reports.MobileReportDraftStatus.ReadyForAttachmentReview, updatedDraft!.Status);
        Assert.Single(updatedDraft.Attachments);
        Assert.Equal(global::App.Mobile.Android.Reports.MobileReportAttachmentKind.Video, updatedDraft.Attachments[0].Kind);
        Assert.Equal(descriptor.FileName, updatedDraft.Attachments[0].FileName);
        Assert.Equal(descriptor.CacheKey, updatedDraft.Attachments[0].SelectedMediaCacheKey);
    }

    [Fact]
    public async Task AttachSelectedVideoAsync_DoesNotRequireBackendFieldsOrBusinessObjectKey()
    {
        var store = CreateStore();
        var draft = await store.CreateFpvDraftAsync();

        var updatedDraft = await store.AttachSelectedVideoAsync(draft.DraftId, CreateDescriptor());

        Assert.NotNull(updatedDraft);
        Assert.DoesNotContain(updatedDraft!.Fields, field =>
            string.Equals(field.FieldKey, "businessObjectKey", StringComparison.OrdinalIgnoreCase));
    }

    private static global::App.Mobile.Android.Services.Local.InMemoryMobileReportDraftStore CreateStore()
    {
        return new global::App.Mobile.Android.Services.Local.InMemoryMobileReportDraftStore(
            new global::App.Mobile.Android.Services.Stubs.StubMobileReportLookupProvider());
    }

    private static global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor CreateDescriptor()
    {
        return new global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor(
            CacheKey: "cache-video-001",
            Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
            FileName: "fpv-video.mp4",
            ContentType: "video/mp4",
            SelectedAtUtc: DateTimeOffset.UtcNow,
            HasLocalReadHandle: true);
    }
}
