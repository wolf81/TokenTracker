using TokenTracker.ViewModels;
using TokenTracker.Views.Base;

namespace TokenTracker.Views
{
    public partial class MenuView : ContentPageBase
    {
        public MenuView()
        {
            InitializeComponent();

            BindingContext = new MenuViewModel();
        }
    }
}
