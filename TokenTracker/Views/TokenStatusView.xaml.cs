﻿using System;
using System.Linq;
using System.Threading.Tasks;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.ViewModels;
using TokenTracker.ViewModels.Base;
using TokenTracker.Views.Base;
using Xamarin.Forms;

namespace TokenTracker.Views
{
    public partial class TokenStatusView : ContentPageBase
    {
        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

        private ITokenCache TokenCache => ViewModelLocator.Resolve<ITokenCache>();

        private ToolbarItem modeToggleItem = new ToolbarItem { Text = "Edit" };

        private TokenStatusViewModel ViewModel => BindingContext as TokenStatusViewModel;

        public TokenStatusView()
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

            await ViewModel.RefreshTokensAsync();

            await ConfigureTokenInfoServiceAsync();

            UpdateModeToggleItem();
            UpdateForCurrentMode();

            addTokenCell.Token = Token.Dummy;

            TokenInfoService.ConnectionStateChanged += Handle_TokenInfoService_ConnectionStateChanged;
        }

        #region Private

        private void Handle_ModeToggleItem_Clicked(object sender, EventArgs e)
        {
            ViewModel.DisplayMode = ViewModel.DisplayMode == DisplayMode.Edit
                ? DisplayMode.View
                : DisplayMode.Edit;

            UpdateModeToggleItem();
            UpdateForCurrentMode();
        }

        private void Handle_TokenInfoService_ConnectionStateChanged(object sender, ConnectionState state)
        {
            Device.BeginInvokeOnMainThread(() => modeToggleItem.IsEnabled = state != ConnectionState.Busy);
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
                    modeToggleItem = new ToolbarItem { Text = "Done" };
                    break;
                case DisplayMode.View:
                    modeToggleItem = new ToolbarItem { Text = "Edit" };
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

        private async Task ConfigureTokenInfoServiceAsync()
        {
            var tokens = await TokenCache.GetTokensAsync();
            var tokenIds = tokens.Select((t) => t.Id);
            TokenInfoService.Configure(tokenIds);
        }

        #endregion
    }
}