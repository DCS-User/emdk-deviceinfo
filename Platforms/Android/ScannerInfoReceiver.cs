using Android.App;
using Android.Content;
using Microsoft.Maui.ApplicationModel;
using Android.OS;
using Application = Android.App.Application;

namespace USB_EMDK.Platforms.Android
{
    /*
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

    */


    public class ScannerInfoReceiver : BroadcastReceiver
    {
        public static event Action<string>? OnSerialReceived;

        public override void OnReceive(Context context, Intent intent)
        {
            System.Diagnostics.Debug.WriteLine($"Receiver got action: {intent.Action}");

            if (intent.Action == "com.symbol.datawedge.api.RESULT_GET_DEVICE_SERIAL_NUMBER")
            {
                string serial = intent.GetStringExtra("com.symbol.datawedge.api.RESULT_GET_DEVICE_SERIAL_NUMBER");
                System.Diagnostics.Debug.WriteLine($"Serial received: {serial}");

                if (!string.IsNullOrEmpty(serial))
                {
                    OnSerialReceived?.Invoke(serial);
                }
            }
        }
    }

    public static class ZebraScannerHelper
    {
        private static ScannerInfoReceiver? _receiver;

        public static void RegisterReceiver()
        {
            var context = Platform.AppContext ?? Application.Context;

            if (_receiver == null)
            {
                _receiver = new ScannerInfoReceiver();
                IntentFilter filter = new IntentFilter();
                filter.AddAction("com.symbol.datawedge.api.RESULT_GET_DEVICE_SERIAL_NUMBER");
                filter.AddAction("com.symbol.datawedge.api.RESULT_ENUMERATE_SCANNERS");

                context.RegisterReceiver(_receiver, filter);
                System.Diagnostics.Debug.WriteLine("Receiver registered");
            }
        }

        public static void UnregisterReceiver()
        {
            var context = Platform.AppContext ?? Application.Context;

            if (_receiver != null)
            {
                context.UnregisterReceiver(_receiver);
                _receiver = null;
            }
        }

        public static void RequestScannerSerial()
        {
            var context = Platform.AppContext ?? Application.Context;

            Intent dwIntent = new Intent();
            dwIntent.SetAction("com.symbol.datawedge.api.ACTION");
            dwIntent.PutExtra("com.symbol.datawedge.api.GET_DEVICE_SERIAL_NUMBER", "");

            // Create result bundle to specify app package
            Bundle resultBundle = new Bundle();
            resultBundle.PutString("RECEIVER_PACKAGE", context.PackageName);
            resultBundle.PutString("RECEIVER_ACTION", "com.symbol.datawedge.api.RESULT_GET_DEVICE_SERIAL_NUMBER");
            dwIntent.PutExtra("SEND_RESULT", resultBundle);

            context.SendBroadcast(dwIntent);
            System.Diagnostics.Debug.WriteLine("Request sent");
        }
    }
}
