using Android.App;
using Android.Runtime;

[assembly: UsesPermission(global::Android.Manifest.Permission.ReadExternalStorage, MaxSdkVersion = 32)]
[assembly: UsesPermission(global::Android.Manifest.Permission.ReadMediaVideo)]
[assembly: UsesPermission(global::Android.Manifest.Permission.Camera)]
[assembly: UsesPermission(global::Android.Manifest.Permission.WriteExternalStorage, MaxSdkVersion = 32)]
[assembly: UsesFeature("android.hardware.camera", Required = false)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]

namespace App.Mobile.Android;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}