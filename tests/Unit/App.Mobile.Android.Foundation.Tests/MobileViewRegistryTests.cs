namespace App.Mobile.Android.Foundation.Tests;

public sealed class MobileViewRegistryTests
{
    private static readonly global::App.Mobile.Android.Navigation.MobileViewId[] ExpectedViewIds =
    [
        global::App.Mobile.Android.Navigation.MobileViewId.Home,
        global::App.Mobile.Android.Navigation.MobileViewId.Upload,
        global::App.Mobile.Android.Navigation.MobileViewId.Queue
    ];

    private static readonly string[] ExpectedRoutes = ["/", "/upload", "/queue"];

    [Fact]
    public void RegistryContainsExactlyHomeUploadAndQueue()
    {
        var registry = new global::App.Mobile.Android.Navigation.MobileViewRegistry();

        var viewIds = registry.MenuEntries.Select(entry => entry.ViewId).ToArray();

        Assert.Equal(ExpectedViewIds, viewIds);
    }

    [Fact]
    public void RegistryUsesExpectedRoutes()
    {
        var registry = new global::App.Mobile.Android.Navigation.MobileViewRegistry();

        var routes = registry.MenuEntries.Select(entry => entry.Route).ToArray();

        Assert.Equal(ExpectedRoutes, routes);
    }

    [Fact]
    public void GetVisibleMenuEntriesReturnsAllThreeEntriesForDefaultStubAccessContext()
    {
        var registry = new global::App.Mobile.Android.Navigation.MobileViewRegistry();
        var accessContext = CreateAccessContext();

        var visibleEntries = registry.GetVisibleMenuEntries(accessContext).ToArray();

        Assert.Equal(3, visibleEntries.Length);
        Assert.Equal(ExpectedRoutes, visibleEntries.Select(entry => entry.Route).ToArray());
    }

    private static global::App.Mobile.Android.Services.Stubs.StubMobileAccessContext CreateAccessContext()
    {
        return new global::App.Mobile.Android.Services.Stubs.StubMobileAccessContext(
            global::Microsoft.Extensions.Options.Options.Create(
                new global::App.Mobile.Android.Options.MobileShellOptions()),
            new global::App.Mobile.Android.Services.Stubs.StubFeatureFlagReader());
    }
}