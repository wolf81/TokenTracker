using Xamarin.Forms;

namespace TokenTracker.Models
{
    public class NavigationMenuItem : MenuItem
    {
        public NavigationMenuItem() { }

        public void Click()
        {
            OnClicked();
        }
    }    
}
