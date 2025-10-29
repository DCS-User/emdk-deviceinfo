using Android.App;
using Android.Content;
using Symbol.XamarinEMDK;
using Symbol.XamarinEMDK.Barcode;
using System.Collections.Generic;

namespace USB_EMDK.Platforms.Android
{
    public class ZebraEmdkHelper : Java.Lang.Object, EMDKManager.IEMDKListener
    {
        private EMDKManager _emdkManager;
        private BarcodeManager _barcodeManager;
        private IList<ScannerInfo> _scanners;

        public event Action<string> DeviceInfoReady;

        public void Init(Context context)
        {
            EMDKResults results = EMDKManager.GetEMDKManager(context, this);
            if (results.StatusCode != EMDKResults.STATUS_CODE.Success)
            {
                DeviceInfoReady?.Invoke("Failed to initialize EMDK");
            }
        }

        public void OnOpened(EMDKManager emdkManager)
        {
            _emdkManager = emdkManager;
            _barcodeManager = (BarcodeManager)_emdkManager.GetInstance(EMDKManager.FEATURE_TYPE.Barcode);

            // Get connected scanners  
            _scanners = _barcodeManager.SupportedDevicesInfo;
            if (_scanners != null && _scanners.Count > 0)
            {
                var info = _scanners[0];
                string details = $"Scanner: {info.DeviceIdentifier}\n" +
                                 $"Friendly Name: {info.FriendlyName}\n" +
                                 $"Connection: {info.GetConnectionType()}\n" +
                                 $"Decoder: {info.GetDecoderType()}";

                DeviceInfoReady?.Invoke(details);
            }
            else
            {
                DeviceInfoReady?.Invoke("No scanner found.");
            }
        }

        public void OnClosed()
        {
            _emdkManager = null;
            _barcodeManager = null;
        }

        public void Release()
        {
           // _barcodeManager?.RemoveConnectionListener(null);
            _emdkManager?.Release();
        }
    }
}
