using System;
using System.Threading.Tasks;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.ViewModels;
using TokenTracker.ViewModels.Base;
using TokenTracker.Views.Base;
using Xamarin.Forms;

namespace TokenTracker.Views
{
    public partial class TokenWalletView : ContentPageBase
    {
        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

        private ToolbarItem modeToggleItem = new ToolbarItem { IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_edit_w.png") };

        private TokenWalletViewModel ViewModel => BindingContext as TokenWalletViewModel;

        public TokenWalletView()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            TokenInfoService.StopTokenUpdates();

            TokenInfoService.ConnectionStateChanged -= Handle_TokenInfoService_ConnectionStateChanged;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //await ViewModel.RefreshTokensAsync();
            await Task.Delay(0);
            //await ConfigureTokenInfoServiceAsync();

            UpdateModeToggleItem();
            UpdateForCurrentMode();

            TokenInfoService.ConnectionStateChanged += Handle_TokenInfoService_ConnectionStateChanged;
        }

        #region Private

        private void Handle_TokenInfoService_ConnectionStateChanged(object sender, ConnectionState state)
        {
            Device.BeginInvokeOnMainThread(() => modeToggleItem.IsEnabled = state != ConnectionState.Busy);
        }

        private void Handle_ModeToggleItem_Clicked(object sender, EventArgs e)
        {
            ViewModel.DisplayMode = ViewModel.DisplayMode == DisplayMode.Edit
                ? DisplayMode.View
                : DisplayMode.Edit;

            UpdateModeToggleItem();
            UpdateForCurrentMode();
        }

        private void UpdateModeToggleItem()
        {
            if (ToolbarItems.Contains(modeToggleItem))
            {
                ToolbarItems.Remove(modeToggleItem);
                modeToggleItem.Clicked -= Handle_ModeToggleItem_Clicked;
            }

            switch (ViewModel.DisplayMode)
            {
                case DisplayMode.Edit:
                    modeToggleItem = new ToolbarItem { IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_checkmark_w.png") };
                    break;
                case DisplayMode.View:
                    modeToggleItem = new ToolbarItem { IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_edit_w.png") };
                    break;
            }

            ToolbarItems.Add(modeToggleItem);
            modeToggleItem.Clicked += Handle_ModeToggleItem_Clicked;
        }

        private void UpdateForCurrentMode()
        {
            switch (ViewModel.DisplayMode)
            {
                case DisplayMode.Edit:
                    TokenInfoService.StopTokenUpdates();
                    break;
                case DisplayMode.View:
                    TokenInfoService.StartTokenUpdates();
                    break;
            }
        }

        #endregion
    }
}
