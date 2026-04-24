using Microsoft.Extensions.Logging;

namespace App.Mobile.Android;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddSingleton<
            global::Microsoft.Extensions.Options.IValidateOptions<global::App.Mobile.Android.Options.MobileShellOptions>,
            global::App.Mobile.Android.Options.MobileShellOptionsValidator>();
        builder.Services
            .AddOptions<global::App.Mobile.Android.Options.MobileShellOptions>()
            .Configure(options =>
            {
                options.DefaultMode = global::App.Mobile.Android.State.MobileShellMode.Development;
                options.ShowShellBanner = true;
            })
            .ValidateOnStart();
        builder.Services.AddSingleton<
            global::App.Mobile.Android.Services.Abstractions.IFeatureFlagReader,
            global::App.Mobile.Android.Services.Stubs.StubFeatureFlagReader>();
        builder.Services.AddSingleton<
            global::App.Mobile.Android.Services.Abstractions.IMobileAccessContext,
            global::App.Mobile.Android.Services.Stubs.StubMobileAccessContext>();
        builder.Services.AddSingleton<
            global::App.Mobile.Android.Services.Abstractions.IMobileMediaService,
            global::App.Mobile.Android.Services.Android.AndroidNativeMediaService>();
        builder.Services.AddSingleton<
            global::App.Mobile.Android.Services.Abstractions.IMobileSelectedMediaStore,
            global::App.Mobile.Android.Services.Local.InMemoryMobileSelectedMediaStore>();
        builder.Services.AddSingleton<global::App.Mobile.Android.Navigation.MobileViewRegistry>();
        builder.Services.AddSingleton<global::App.Mobile.Android.Navigation.MobileNavigationState>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}