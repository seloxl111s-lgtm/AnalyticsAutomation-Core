namespace App.Mobile.Android.Navigation;

internal sealed record MobileMenuEntry(
    MobileViewId ViewId,
    string Title,
    string Route,
    string IconCssClass);
