namespace App.Mobile.Android.Foundation.Tests;

public sealed class MobileNavigationStateTests
{
    [Fact]
    public void SetCurrentViewUpdatesCurrentViewId()
    {
        var state = new global::App.Mobile.Android.Navigation.MobileNavigationState();

        state.SetCurrentView(global::App.Mobile.Android.Navigation.MobileViewId.Queue);

        Assert.Equal(global::App.Mobile.Android.Navigation.MobileViewId.Queue, state.CurrentViewId);
    }
}