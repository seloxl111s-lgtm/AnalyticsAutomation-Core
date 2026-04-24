namespace App.Mobile.Android.Localization;

internal static class MobileUiText
{
    public const string ApplicationTitle = "Мобильный клиент";
    public const string NavigationSubtitle = "Локальная оболочка Android";
    public const string ShellBannerLabel = "Локальный режим";

    public const string MenuHome = "Главная";
    public const string MenuUpload = "Загрузка";
    public const string MenuQueue = "Очередь";

    public const string HomeTitle = "Главная";
    public const string HomeIntro = "Текущая Android-оболочка работает локально и показывает только базовую мобильную навигацию без backend-интеграции.";

    public const string UploadTitle = "Загрузка";
    public const string UploadIntro = "На этом срезе доступны только локальный выбор и запись видео на устройстве. Файлы не копируются, не сохраняются отдельно и не отправляются в backend.";
    public const string UploadCapabilityCardTitle = "Локальные media-возможности устройства";
    public const string UploadCapabilityCardSummary = "Текущий Android baseline открывает системный выбор видео и запись видео на устройстве, если камера поддерживается. Это только local device media без upload и sync.";
    public const string UploadLocalDeviceBadge = "Только на устройстве";
    public const string UploadFilePickerLabel = "Файловый seam";
    public const string UploadGalleryVideoLabel = "Выбор видео из галереи";
    public const string UploadCameraCaptureLabel = "Запись видео с камеры";
    public const string UploadSelectVideoButton = "Выбрать видео";
    public const string UploadCaptureVideoButton = "Записать видео";
    public const string UploadLoadingCapabilitySnapshot = "Загружается локальная информация о media-возможностях...";
    public const string UploadNativePickerTitle = "Выберите одно видео";
    public const string UploadNativeCaptureTitle = "Запишите одно видео";
    public const string UploadSelectedVideoFallbackName = "без имени";
    public const string UploadNativePickerCancelledText = "Выбор видео отменён. Никакие файлы не были сохранены или отправлены.";
    public const string UploadNativePickerUnavailableText = "Не удалось открыть системный выбор видео на устройстве. Проверьте разрешения и повторите попытку.";
    public const string UploadNativePickerFailedText = "Не удалось завершить локальный выбор видео. Это только Android media baseline без upload и sync.";
    public const string UploadNativeCaptureCancelledText = "Запись видео отменена. Никакие файлы не были сохранены или отправлены.";
    public const string UploadNativeCaptureUnavailableText = "Системная запись видео недоступна на этом устройстве или сейчас не поддерживается.";
    public const string UploadNativeCapturePermissionDeniedText = "Доступ к камере не предоставлен. Разрешите использование камеры и повторите попытку.";
    public const string UploadNativeCaptureFailedText = "Не удалось завершить локальную запись видео. Это только Android media baseline без upload и sync.";
    public const string UploadSelectedMediaCardTitle = "Текущий локально выбранный медиафайл";
    public const string UploadSelectedMediaSourceLabel = "Источник";
    public const string UploadSelectedMediaFileNameLabel = "Имя файла";
    public const string UploadSelectedMediaContentTypeLabel = "MIME-тип";
    public const string UploadSelectedMediaSelectedAtLabel = "Выбрано";
    public const string UploadSelectedMediaStateLabel = "Состояние";
    public const string UploadSelectedMediaStateText = "Локальный доступ к файлу активен только в текущем запуске, без upload и sync";
    public const string UploadClearSelectionButton = "Очистить локальный выбор";
    public const string UploadClearSelectionResultText = "Локальный выбранный медиафайл очищен.";
    public const string UploadUnknownContentTypeText = "Неизвестно";

    public const string QueueTitle = "Очередь";
    public const string QueueIntro = "Это локальный экран-заглушка для будущей очереди. На этом срезе нет offline queue, sync и бизнес-действий.";

    public const string NotFoundTitle = "Страница не найдена";
    public const string NotFoundMessage = "Запрошенный экран не найден в текущей локальной оболочке.";

    public static string GetShellModeText(global::App.Mobile.Android.State.MobileShellMode mode)
    {
        return mode switch
        {
            global::App.Mobile.Android.State.MobileShellMode.Development => "Разработка",
            global::App.Mobile.Android.State.MobileShellMode.GuestPlaceholder => "Гостевой режим-заглушка",
            global::App.Mobile.Android.State.MobileShellMode.OfflineRestrictedPlaceholder => "Ограниченный офлайн-режим-заглушка",
            _ => "Неизвестный режим"
        };
    }

    public static string GetShellBannerText(global::App.Mobile.Android.State.MobileShellMode mode)
    {
        return $"Локальное состояние оболочки: {GetShellModeText(mode)}. Это только локальная заглушка shell-state, а не реальная auth/session/backend интеграция.";
    }

    public static string GetMediaCapabilityStateText(global::App.Mobile.Android.Media.MobileMediaCapabilityState state)
    {
        return state switch
        {
            global::App.Mobile.Android.Media.MobileMediaCapabilityState.Unknown => "Неизвестно",
            global::App.Mobile.Android.Media.MobileMediaCapabilityState.StubReady => "Готово только как заглушка",
            global::App.Mobile.Android.Media.MobileMediaCapabilityState.DeviceAvailable => "Доступно на устройстве",
            global::App.Mobile.Android.Media.MobileMediaCapabilityState.NotBoundYet => "Пока не подключено в этом срезе",
            _ => "Неизвестно"
        };
    }

    public static string GetSelectedMediaSourceText(global::App.Mobile.Android.Media.MobileMediaSource source)
    {
        return source switch
        {
            global::App.Mobile.Android.Media.MobileMediaSource.FilePicker => "Файловый выбор",
            global::App.Mobile.Android.Media.MobileMediaSource.GalleryVideo => "Галерея",
            global::App.Mobile.Android.Media.MobileMediaSource.CameraCapture => "Камера",
            _ => "Неизвестно"
        };
    }

    public static string GetUploadNativePickerSuccessText(string fileName)
    {
        return $"Выбрано видео «{fileName}». Файл не был скопирован, сохранён отдельно или отправлен.";
    }

    public static string GetUploadNativeCaptureSuccessText(string fileName)
    {
        return $"Записано видео «{fileName}». Файл не был скопирован, сохранён отдельно или отправлен.";
    }
}