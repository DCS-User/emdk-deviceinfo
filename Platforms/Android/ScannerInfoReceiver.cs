using Android.App;
using Android.Content;
using Microsoft.Maui.ApplicationModel;
using Android.OS;
using Application = Android.App.Application;

namespace USB_EMDK.Platforms.Android
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] {
        "com.symbol.datawedge.api.RESULT_ENUMERATE_SCANNERS",
        "com.symbol.datawedge.api.RESULT_GET_DEVICE_SERIAL_NUMBER"
    })]
    public class ScannerInfoReceiver : BroadcastReceiver
    {
        public static event Action<string>? OnSerialReceived;
        public static event Action<List<string>>? OnScannersEnumerated;

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == "com.symbol.datawedge.api.RESULT_ENUMERATE_SCANNERS")
            {
                var scanners = intent.GetParcelableArrayExtra("com.symbol.datawedge.api.RESULT_ENUMERATE_SCANNERS");
                if (scanners == null) return;

                var scannerNames = new List<string>();
                foreach (var s in scanners)
                {
                    if (s is Bundle b)
                    {
                        string scannerName = b.GetString("SCANNER_NAME");
                        if (!string.IsNullOrEmpty(scannerName))
                        {
                            scannerNames.Add(scannerName);
                        }
                    }
                }
                OnScannersEnumerated?.Invoke(scannerNames);
            }
            else if (intent.Action == "com.symbol.datawedge.api.RESULT_GET_DEVICE_SERIAL_NUMBER")
            {
                string serial = intent.GetStringExtra("com.symbol.datawedge.api.RESULT_GET_DEVICE_SERIAL_NUMBER");
                if (!string.IsNullOrEmpty(serial))
                {
                    OnSerialReceived?.Invoke(serial);
                }
            }
        }
    }

    public static class ZebraScannerHelper
    {
        public static void RequestScannerSerial()
        {
            var context = Platform.AppContext ?? Application.Context;
            Intent dwIntent = new Intent();
            dwIntent.SetAction("com.symbol.datawedge.api.ACTION");
            dwIntent.PutExtra("com.symbol.datawedge.api.GET_DEVICE_SERIAL_NUMBER", "");
            context.SendBroadcast(dwIntent);
        }

        public static void EnumerateScanners()
        {
            var context = Platform.AppContext ?? Application.Context;
            Intent dwIntent = new Intent();
            dwIntent.SetAction("com.symbol.datawedge.api.ACTION");
            dwIntent.PutExtra("com.symbol.datawedge.api.ENUMERATE_SCANNERS", "");
            context.SendBroadcast(dwIntent);
        }
    }
}