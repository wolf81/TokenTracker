using Xamarin.Forms;

namespace TokenTracker.Views
{
    public partial class CustomNavigationView : NavigationPage
    {
        public CustomNavigationView()
        {
            InitializeComponent();
        }

        public CustomNavigationView(Page root) : base(root)
        {
            InitializeComponent();

            Title = root.Title;
            IconImageSource = root.IconImageSource;
        }
    }
}
