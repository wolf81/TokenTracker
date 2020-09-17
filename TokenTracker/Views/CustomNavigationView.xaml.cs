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

        public void OnTabHidden()
        {
            if (CurrentPage is ITabbedViewAppearanceAware tabbedViewAppearanceAware)
            {
                tabbedViewAppearanceAware.OnTabHidden();
            }
        }

        public void OnTabShown()
        {
            if (CurrentPage is ITabbedViewAppearanceAware tabbedViewAppearanceAware)
            {
                tabbedViewAppearanceAware.OnTabShown();
            }
        }

        #endregion
    }
}
