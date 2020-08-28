using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TokenTracker.Views.Base
{
    public class ContentPageBase : ContentPage
    {
        public ContentPageBase()
        {
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}

