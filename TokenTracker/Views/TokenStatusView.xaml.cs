using System;
using TokenTracker.Models;
using TokenTracker.Utilities;
using TokenTracker.ViewModels;
using TokenTracker.Views.Base;
using Xamarin.Forms;

namespace TokenTracker.Views
{
    public partial class TokenStatusView : ContentPageBase, ITabbedViewAppearanceAware
    {
        private TokenStatusViewModel ViewModel => BindingContext as TokenStatusViewModel;
        
        public TokenStatusView()
        {
            InitializeComponent();

            addTokenCell.Token = Token.Dummy;

            ShowConnectionStatusView = true;
        }

        #region ITabbedViewAppearanceAware

        public async void OnTabShown()
        {
            await ViewModel.ConfigureTokenInfoServiceAsync();

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
            if (RightNavigationItem != null)
            {
                RightNavigationItem.Clicked -= Handle_ModeToggleItem_Clicked;
                RightNavigationItem = null;
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
