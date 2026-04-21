namespace App.Mobile.Android.Media;

internal sealed record LocalSelectedMediaDescriptor(
    string CacheKey,
    MobileMediaSource Source,
    string FileName,
    string? ContentType,
    DateTimeOffset SelectedAtUtc,
    bool HasLocalReadHandle);
