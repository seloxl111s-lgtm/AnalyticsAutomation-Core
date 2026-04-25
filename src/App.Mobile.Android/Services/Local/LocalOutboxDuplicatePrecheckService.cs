namespace App.Mobile.Android.Services.Local;

internal sealed class LocalOutboxDuplicatePrecheckService :
    global::App.Mobile.Android.Services.Abstractions.ILocalDuplicatePrecheckService
{
    public global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckResult CheckAgainstOutbox(
        global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor? currentSelection,
        IReadOnlyList<global::App.Mobile.Android.Outbox.PendingSyncItem> existingItems)
    {
        ArgumentNullException.ThrowIfNull(existingItems);

        if (currentSelection is null)
        {
            return new global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckResult(
                Status: global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckStatus.NoCurrentSelection,
                CanEnqueue: false,
                Message: global::App.Mobile.Android.Localization.MobileUiText.LocalDuplicatePrecheckNoCurrentSelectionText);
        }

        foreach (var item in existingItems)
        {
            var draft = item.LocalMediaDraft;
            if (draft is null)
            {
                continue;
            }

            if (string.Equals(draft.CacheKey, currentSelection.CacheKey, StringComparison.Ordinal))
            {
                return CreateLikelyDuplicateResult();
            }

            var hasSameSource = draft.Source == currentSelection.Source;
            var hasSameFileName = string.Equals(
                draft.FileName,
                currentSelection.FileName,
                StringComparison.OrdinalIgnoreCase);
            var hasSameContentType = string.Equals(
                NormalizeContentType(draft.ContentType),
                NormalizeContentType(currentSelection.ContentType),
                StringComparison.Ordinal);

            if (hasSameSource && hasSameFileName && hasSameContentType)
            {
                return CreateLikelyDuplicateResult();
            }
        }

        return new global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckResult(
            Status: global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckStatus.NoKnownDuplicateInOutbox,
            CanEnqueue: true,
            Message: global::App.Mobile.Android.Localization.MobileUiText.LocalDuplicatePrecheckNoKnownDuplicateText);
    }

    private static global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckResult CreateLikelyDuplicateResult()
    {
        return new global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckResult(
            Status: global::App.Mobile.Android.DuplicatePrecheck.LocalDuplicatePrecheckStatus.LikelyAlreadyQueued,
            CanEnqueue: false,
            Message: global::App.Mobile.Android.Localization.MobileUiText.LocalDuplicatePrecheckLikelyAlreadyQueuedText);
    }

    private static string NormalizeContentType(string? contentType)
    {
        return string.IsNullOrWhiteSpace(contentType)
            ? string.Empty
            : contentType.Trim().ToUpperInvariant();
    }
}