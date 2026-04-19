namespace App.Mobile.Android.Options;

internal sealed class MobileShellOptionsValidator :
    global::Microsoft.Extensions.Options.IValidateOptions<MobileShellOptions>
{
    public global::Microsoft.Extensions.Options.ValidateOptionsResult Validate(
        string? name,
        MobileShellOptions options)
    {
        if (!global::System.Enum.IsDefined(options.DefaultMode))
        {
            return global::Microsoft.Extensions.Options.ValidateOptionsResult.Fail(
                "MobileShellOptions.DefaultMode must be a defined MobileShellMode value.");
        }

        return global::Microsoft.Extensions.Options.ValidateOptionsResult.Success;
    }
}
