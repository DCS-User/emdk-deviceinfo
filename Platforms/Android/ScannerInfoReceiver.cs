using Android.App;
using Android.Content;
using Microsoft.Maui.ApplicationModel;
using Android.OS;

namespace USB_EMDK.Platforms.Android
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "com.symbol.datawedge.api.RESULT_ENUMERATE_SCANNERS" })]
    public class ScannerInfoReceiver : BroadcastReceiver
    {
        public static event Action<string>? OnSerialReceived;

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == "com.symbol.datawedge.api.RESULT_ENUMERATE_SCANNERS")
            {
                var scanners = intent.GetParcelableArrayExtra("com.symbol.datawedge.api.RESULT_ENUMERATE_SCANNERS");
                if (scanners == null) return;

                foreach (var s in scanners)
                {
                    if (s is Bundle b)
                    {
                        string serial = b.GetString("SERIAL_NUMBER");
                        if (!string.IsNullOrEmpty(serial))
                        {
                            OnSerialReceived?.Invoke(serial);
                            break; // take first connected scanner
                        }
                    }
                }
            }
        }
    }
    public static class ZebraScannerHelper
    {
        public static void RequestScannerSerial()
        {
            var context = Platform.AppContext ?? Android.App.Application.Context;

            Intent dwIntent = new Intent();
            dwIntent.SetAction("com.symbol.datawedge.api.ACTION");
            dwIntent.PutExtra("com.symbol.datawedge.api.ENUMERATE_SCANNERS", "");

            context.SendBroadcast(dwIntent);
        }
    }
}
