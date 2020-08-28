using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TokenTracker.Views.Base
{
    public class ContentPageBase : ContentPage
    {
        public ContentPageBase()
        {
            BackgroundColor = Color.FromHex("#e7e7e7");

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}

