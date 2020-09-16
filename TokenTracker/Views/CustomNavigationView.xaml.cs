using TokenTracker.Utilities;
using Xamarin.Forms;

namespace TokenTracker.Views
{
    public partial class CustomNavigationView : NavigationPage, ITabbedViewAppearanceAware
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

        #region ITabbedViewAppearanceAware

        public void TabSelected()
        {
            if (CurrentPage is ITabbedViewAppearanceAware tabbedViewAppearanceAware)
            {
                tabbedViewAppearanceAware.TabSelected();
            }
        }

        #endregion
    }
}
