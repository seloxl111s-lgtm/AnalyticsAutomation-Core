namespace App.Mobile.Android.Outbox;

internal sealed record PendingSyncItemLocalMediaDraft(
    string CacheKey,
    global::App.Mobile.Android.Media.MobileMediaSource Source,
    string FileName,
    string? ContentType,
    DateTimeOffset SelectedAtUtc,
    bool HasLocalReadHandle);