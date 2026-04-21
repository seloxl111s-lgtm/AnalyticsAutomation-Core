namespace App.Mobile.Android.Services.Local;

internal sealed class LocalCurrentSelectionDraftRepairService :
    global::App.Mobile.Android.Services.Abstractions.ILocalMediaDraftRepairService
{
    public global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairResult CheckCurrentSelectionForDraftRepair(
        global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor? currentSelection,
        global::App.Mobile.Android.Outbox.PendingSyncItem? targetItem)
    {
        if (currentSelection is null)
        {
            return new global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairResult(
                Status: global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.NoCurrentSelection,
                CanApply: false,
                Message: global::App.Mobile.Android.Localization.MobileUiText.QueueRepairNoCurrentSelectionText);
        }

        if (!currentSelection.HasLocalReadHandle)
        {
            return new global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairResult(
                Status: global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.SelectionHasNoLiveHandle,
                CanApply: false,
                Message: global::App.Mobile.Android.Localization.MobileUiText.QueueRepairSelectionHasNoLiveHandleText);
        }

        var targetDraft = targetItem?.LocalMediaDraft;
        if (targetDraft is null)
        {
            return new global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairResult(
                Status: global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.NoRepairableDraft,
                CanApply: false,
                Message: global::App.Mobile.Android.Localization.MobileUiText.QueueRepairNoRepairableDraftText);
        }

        if (targetDraft.HasLocalReadHandle)
        {
            return new global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairResult(
                Status: global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.DraftAlreadyHasLiveHandle,
                CanApply: false,
                Message: global::App.Mobile.Android.Localization.MobileUiText.QueueRepairAlreadyRepairedText);
        }

        if (string.Equals(targetDraft.CacheKey, currentSelection.CacheKey, StringComparison.Ordinal))
        {
            return new global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairResult(
                Status: global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.ReadyToRepair,
                CanApply: true,
                Message: global::App.Mobile.Android.Localization.MobileUiText.QueueRepairReadyText);
        }

        var hasSameFileName = string.Equals(
            targetDraft.FileName,
            currentSelection.FileName,
            StringComparison.OrdinalIgnoreCase);
        var hasSameContentType = string.Equals(
            NormalizeContentType(targetDraft.ContentType),
            NormalizeContentType(currentSelection.ContentType),
            StringComparison.Ordinal);

        if (hasSameFileName && hasSameContentType)
        {
            return new global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairResult(
                Status: global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.ReadyToRepair,
                CanApply: true,
                Message: global::App.Mobile.Android.Localization.MobileUiText.QueueRepairReadyText);
        }

        return new global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairResult(
            Status: global::App.Mobile.Android.DraftRepair.LocalMediaDraftRepairStatus.SelectionDoesNotMatchDraft,
            CanApply: false,
            Message: global::App.Mobile.Android.Localization.MobileUiText.QueueRepairSelectionDoesNotMatchText);
    }

    private static string NormalizeContentType(string? contentType)
    {
        return string.IsNullOrWhiteSpace(contentType)
            ? string.Empty
            : contentType.Trim().ToUpperInvariant();
    }
}
