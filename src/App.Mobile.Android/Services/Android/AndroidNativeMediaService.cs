namespace App.Mobile.Android.Services.Android;

internal sealed class AndroidNativeMediaService :
    global::App.Mobile.Android.Services.Abstractions.IMobileMediaService
{
    private readonly global::App.Mobile.Android.Services.Abstractions.IMobileSelectedMediaStore _selectedMediaStore;

    public AndroidNativeMediaService(
        global::App.Mobile.Android.Services.Abstractions.IMobileSelectedMediaStore selectedMediaStore)
    {
        _selectedMediaStore = selectedMediaStore;
    }

    public Task<global::App.Mobile.Android.Media.MobileMediaCapabilitySnapshot> GetCapabilitySnapshotAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var isCaptureSupported = global::Microsoft.Maui.Media.MediaPicker.Default.IsCaptureSupported;

        return Task.FromResult(
            new global::App.Mobile.Android.Media.MobileMediaCapabilitySnapshot(
                FilePickerSeamState: global::App.Mobile.Android.Media.MobileMediaCapabilityState.NotBoundYet,
                GalleryVideoSeamState: global::App.Mobile.Android.Media.MobileMediaCapabilityState.DeviceAvailable,
                CameraCaptureSeamState: isCaptureSupported
                    ? global::App.Mobile.Android.Media.MobileMediaCapabilityState.DeviceAvailable
                    : global::App.Mobile.Android.Media.MobileMediaCapabilityState.NotBoundYet,
                SummaryText: global::App.Mobile.Android.Localization.MobileUiText.UploadCapabilityCardSummary));
    }

    public async Task<global::App.Mobile.Android.Media.MobileMediaOperationResult> PickVideoAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var options = new global::Microsoft.Maui.Media.MediaPickerOptions
            {
                Title = global::App.Mobile.Android.Localization.MobileUiText.UploadNativePickerTitle,
                SelectionLimit = 1
            };

            var pickedVideos =
                await global::Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(
                    () => global::Microsoft.Maui.Media.MediaPicker.Default.PickVideosAsync(options));

            var selectedVideo = pickedVideos is null
                ? null
                : global::System.Linq.Enumerable.FirstOrDefault(pickedVideos);

            if (selectedVideo is null)
            {
                return new global::App.Mobile.Android.Media.MobileMediaOperationResult(
                    Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
                    Status: global::App.Mobile.Android.Media.MobileMediaOperationStatus.Unknown,
                    Message: global::App.Mobile.Android.Localization.MobileUiText.UploadNativePickerCancelledText);
            }

            var selectedFileName = string.IsNullOrWhiteSpace(selectedVideo.FileName)
                ? global::App.Mobile.Android.Localization.MobileUiText.UploadSelectedVideoFallbackName
                : selectedVideo.FileName;

            await CacheSelectedMediaAsync(
                selectedVideo,
                global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
                cancellationToken);

            return new global::App.Mobile.Android.Media.MobileMediaOperationResult(
                Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
                Status: global::App.Mobile.Android.Media.MobileMediaOperationStatus.DeviceSelectionCompleted,
                Message: global::App.Mobile.Android.Localization.MobileUiText.GetUploadNativePickerSuccessText(selectedFileName));
        }
        catch (global::Microsoft.Maui.ApplicationModel.FeatureNotSupportedException)
        {
            return new global::App.Mobile.Android.Media.MobileMediaOperationResult(
                Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
                Status: global::App.Mobile.Android.Media.MobileMediaOperationStatus.NotAvailable,
                Message: global::App.Mobile.Android.Localization.MobileUiText.UploadNativePickerUnavailableText);
        }
        catch (global::Microsoft.Maui.ApplicationModel.PermissionException)
        {
            return new global::App.Mobile.Android.Media.MobileMediaOperationResult(
                Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
                Status: global::App.Mobile.Android.Media.MobileMediaOperationStatus.NotAvailable,
                Message: global::App.Mobile.Android.Localization.MobileUiText.UploadNativePickerUnavailableText);
        }
        catch (Exception)
        {
            return new global::App.Mobile.Android.Media.MobileMediaOperationResult(
                Source: global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo,
                Status: global::App.Mobile.Android.Media.MobileMediaOperationStatus.NotAvailable,
                Message: global::App.Mobile.Android.Localization.MobileUiText.UploadNativePickerFailedText);
        }
    }

    public Task<global::App.Mobile.Android.Media.MobileMediaOperationResult> CaptureVideoAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!global::Microsoft.Maui.Media.MediaPicker.Default.IsCaptureSupported)
        {
            return Task.FromResult(
                new global::App.Mobile.Android.Media.MobileMediaOperationResult(
                    Source: global::App.Mobile.Android.Media.MobileMediaSource.CameraCapture,
                    Status: global::App.Mobile.Android.Media.MobileMediaOperationStatus.NotAvailable,
                    Message: global::App.Mobile.Android.Localization.MobileUiText.UploadNativeCaptureUnavailableText));
        }

        return CaptureVideoCoreAsync(cancellationToken);
    }

    private async Task<global::App.Mobile.Android.Media.MobileMediaOperationResult> CaptureVideoCoreAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var options = new global::Microsoft.Maui.Media.MediaPickerOptions
            {
                Title = global::App.Mobile.Android.Localization.MobileUiText.UploadNativeCaptureTitle
            };

            var capturedVideo =
                await global::Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(
                    () => global::Microsoft.Maui.Media.MediaPicker.Default.CaptureVideoAsync(options));

            if (capturedVideo is null)
            {
                return new global::App.Mobile.Android.Media.MobileMediaOperationResult(
                    Source: global::App.Mobile.Android.Media.MobileMediaSource.CameraCapture,
                    Status: global::App.Mobile.Android.Media.MobileMediaOperationStatus.Unknown,
                    Message: global::App.Mobile.Android.Localization.MobileUiText.UploadNativeCaptureCancelledText);
            }

            var capturedFileName = string.IsNullOrWhiteSpace(capturedVideo.FileName)
                ? global::App.Mobile.Android.Localization.MobileUiText.UploadSelectedVideoFallbackName
                : capturedVideo.FileName;

            await CacheSelectedMediaAsync(
                capturedVideo,
                global::App.Mobile.Android.Media.MobileMediaSource.CameraCapture,
                cancellationToken);

            return new global::App.Mobile.Android.Media.MobileMediaOperationResult(
                Source: global::App.Mobile.Android.Media.MobileMediaSource.CameraCapture,
                Status: global::App.Mobile.Android.Media.MobileMediaOperationStatus.DeviceCaptureCompleted,
                Message: global::App.Mobile.Android.Localization.MobileUiText.GetUploadNativeCaptureSuccessText(capturedFileName));
        }
        catch (global::Microsoft.Maui.ApplicationModel.FeatureNotSupportedException)
        {
            return new global::App.Mobile.Android.Media.MobileMediaOperationResult(
                Source: global::App.Mobile.Android.Media.MobileMediaSource.CameraCapture,
                Status: global::App.Mobile.Android.Media.MobileMediaOperationStatus.NotAvailable,
                Message: global::App.Mobile.Android.Localization.MobileUiText.UploadNativeCaptureUnavailableText);
        }
        catch (global::Microsoft.Maui.ApplicationModel.PermissionException)
        {
            return new global::App.Mobile.Android.Media.MobileMediaOperationResult(
                Source: global::App.Mobile.Android.Media.MobileMediaSource.CameraCapture,
                Status: global::App.Mobile.Android.Media.MobileMediaOperationStatus.NotAvailable,
                Message: global::App.Mobile.Android.Localization.MobileUiText.UploadNativeCapturePermissionDeniedText);
        }
        catch (Exception)
        {
            return new global::App.Mobile.Android.Media.MobileMediaOperationResult(
                Source: global::App.Mobile.Android.Media.MobileMediaSource.CameraCapture,
                Status: global::App.Mobile.Android.Media.MobileMediaOperationStatus.NotAvailable,
                Message: global::App.Mobile.Android.Localization.MobileUiText.UploadNativeCaptureFailedText);
        }
    }

    private Task CacheSelectedMediaAsync(
        global::Microsoft.Maui.Storage.FileResult file,
        global::App.Mobile.Android.Media.MobileMediaSource source,
        CancellationToken cancellationToken)
    {
        var descriptor = new global::App.Mobile.Android.Media.LocalSelectedMediaDescriptor(
            CacheKey: global::System.Guid.NewGuid().ToString("N"),
            Source: source,
            FileName: string.IsNullOrWhiteSpace(file.FileName)
                ? global::App.Mobile.Android.Localization.MobileUiText.UploadSelectedVideoFallbackName
                : file.FileName,
            ContentType: file.ContentType,
            SelectedAtUtc: global::System.DateTimeOffset.UtcNow,
            HasLocalReadHandle: true);

        return _selectedMediaStore.CacheAsync(
            descriptor,
            file.OpenReadAsync,
            cancellationToken);
    }
}