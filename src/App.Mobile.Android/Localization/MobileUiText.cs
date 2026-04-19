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
    public const string UploadIntro = "Это локальный экран-заглушка для будущего upload-потока. На этом срезе нет auth, media binding и серверных действий.";

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
}
