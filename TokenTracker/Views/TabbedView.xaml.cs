using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TokenTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedView : TabbedPage
    {
        public TabbedView()
        {
            InitializeComponent();
        }
    }
}
