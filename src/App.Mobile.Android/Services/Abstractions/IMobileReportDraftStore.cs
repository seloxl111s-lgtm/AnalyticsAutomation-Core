namespace App.Mobile.Android.Services.Abstractions;

internal interface IMobileReportDraftStore
{
    Task<IReadOnlyList<global::App.Mobile.Android.Reports.MobileReportDraft>> GetDraftsAsync(
        CancellationToken cancellationToken = default);

    Task<global::App.Mobile.Android.Reports.MobileReportDraft?> GetDraftAsync(
        string draftId,
        CancellationToken cancellationToken = default);

    Task<global::App.Mobile.Android.Reports.MobileReportDraft> CreateFpvDraftAsync(
        CancellationToken cancellationToken = default);

    Task<global::App.Mobile.Android.Reports.MobileReportDraft?> AttachSelectedVideoAsync(
        string draftId,
        global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor descriptor,
        CancellationToken cancellationToken = default);
}
