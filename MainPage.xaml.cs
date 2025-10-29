#if ANDROID
using Android.Content.Res;
using USB_EMDK.Platforms.Android;
#endif
namespace USB_EMDK
{
    public partial class MainPage : ContentPage
    {
#if ANDROID
        private ZebraEmdkHelper _emdkHelper;
#endif

        public MainPage()
        {
            InitializeComponent();
#if ANDROID
            ScannerInfoReceiver.OnSerialReceived += serial =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Scanner Serial", serial, "OK");
                });
            };
#endif
        }

        private void OnFetchClicked(object sender, EventArgs e)
        {
#if ANDROID
            ZebraScannerHelper.RequestScannerSerial();
#endif
        }
    }

}
