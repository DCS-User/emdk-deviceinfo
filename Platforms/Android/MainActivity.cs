using Android.App;
using Android.Content.PM;
using Android.OS;
using USB_EMDK.Platforms.Android;

namespace USB_EMDK
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        // In MainActivity.cs OnCreate or App initialization
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ZebraScannerHelper.RegisterReceiver();

            // Subscribe to event
            ScannerInfoReceiver.OnSerialReceived += (serial) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Update UI with serial
                    System.Diagnostics.Debug.WriteLine($"Got serial: {serial}");
                });
            };
        }

        protected override void OnDestroy()
        {
            ZebraScannerHelper.UnregisterReceiver();
            base.OnDestroy();
        }
    }
}
