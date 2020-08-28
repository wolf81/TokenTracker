using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TokenTracker.Views.Base
{
    public class ContentPageBase : ContentPage
    {
        public ContentPageBase()
        {
            //On<iOS>().SetUseSafeArea(true);

            Content = new StackLayout
            {
                Children = {
                    new ContentView { },
                }
            };
        }
    }
}

