namespace App.Mobile.Android.Media;

internal sealed record MobileMediaCapabilitySnapshot(
    MobileMediaCapabilityState FilePickerSeamState,
    MobileMediaCapabilityState GalleryVideoSeamState,
    MobileMediaCapabilityState CameraCaptureSeamState,
    string SummaryText);