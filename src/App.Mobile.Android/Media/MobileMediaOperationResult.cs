namespace App.Mobile.Android.Media;

internal sealed record MobileMediaOperationResult(
    MobileMediaSource Source,
    MobileMediaOperationStatus Status,
    string Message);
