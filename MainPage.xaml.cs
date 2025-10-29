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
            _emdkHelper = new ZebraEmdkHelper();
            _emdkHelper.DeviceInfoReady += (info) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    LblInfo.Text = info;
                });
            };
#endif
        }

        private void OnFetchClicked(object sender, EventArgs e)
        {
#if ANDROID
            _emdkHelper.Init(Android.App.Application.Context);
#endif
        }
    }

}
