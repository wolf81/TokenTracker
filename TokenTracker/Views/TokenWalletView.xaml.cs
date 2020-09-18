using System;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.Utilities;
using TokenTracker.ViewModels;
using TokenTracker.ViewModels.Base;
using TokenTracker.Views.Base;
using Xamarin.Forms;

namespace TokenTracker.Views
{
    public partial class TokenWalletView : ContentPageBase, ITabbedViewAppearanceAware
    {
        private TokenWalletViewModel ViewModel => BindingContext as TokenWalletViewModel;

        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

        public TokenWalletView()
        {
            InitializeComponent();

            ShowConnectionStatusView = true;
        }

        #region ITabbedViewAppearanceAware

        public void OnTabShown()
        {
            if (TokenInfoService.IsConfigured)
            {
                TokenInfoService.StartTokenUpdates();
            }

            UpdateModeToggleItem();
        }

        public void OnTabHidden() { }

        #endregion

        #region Private

        private void Handle_ModeToggleItem_Clicked(object sender, EventArgs e)
        {
            ViewModel.DisplayMode = ViewModel.DisplayMode == DisplayMode.Edit
                ? DisplayMode.View
                : DisplayMode.Edit;

            UpdateModeToggleItem();
        }

        private void UpdateModeToggleItem()
        {
            if (RightNavigationItem is NavigationMenuItem item)
            {                
                item.Clicked -= Handle_ModeToggleItem_Clicked;
                item = null;
            }

            switch (ViewModel.DisplayMode)
            {
                case DisplayMode.Edit:
                    RightNavigationItem = new NavigationMenuItem { IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_checkmark_w.png") };
                    break;
                case DisplayMode.View:
                    RightNavigationItem = new NavigationMenuItem { IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_edit_w.png") };
                    break;
            }

            RightNavigationItem.Clicked += Handle_ModeToggleItem_Clicked;
        }

        #endregion
    }
}
