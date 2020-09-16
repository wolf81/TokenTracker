using System;
using TokenTracker.Models;
using TokenTracker.ViewModels;
using TokenTracker.Views.Base;
using Xamarin.Forms;

namespace TokenTracker.Views
{
    public partial class TokenStatusView : ContentPageBase
    {
        private TokenStatusViewModel ViewModel => BindingContext as TokenStatusViewModel;
        
        public TokenStatusView()
        {
            InitializeComponent();

            addTokenCell.Token = Token.Dummy;

            ShowConnectionStatusView = true;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ViewModel.ConfigureTokenInfoServiceAsync();

            UpdateModeToggleItem();
        }

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
