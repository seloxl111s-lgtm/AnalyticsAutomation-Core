namespace App.Mobile.Android.Foundation.Tests;

public sealed class MobileShellOptionsValidatorTests
{
    [Fact]
    public void ValidateWithDefinedDefaultModeSucceeds()
    {
        var validator = new global::App.Mobile.Android.Options.MobileShellOptionsValidator();
        var options = new global::App.Mobile.Android.Options.MobileShellOptions
        {
            DefaultMode = global::App.Mobile.Android.State.MobileShellMode.Development
        };

        var result = validator.Validate(name: null, options);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public void ValidateWithUndefinedDefaultModeFails()
    {
        var validator = new global::App.Mobile.Android.Options.MobileShellOptionsValidator();
        var options = new global::App.Mobile.Android.Options.MobileShellOptions
        {
            DefaultMode = (global::App.Mobile.Android.State.MobileShellMode)999
        };

        var result = validator.Validate(name: null, options);

        Assert.True(result.Failed);
    }
}