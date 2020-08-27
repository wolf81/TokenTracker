using System.Collections.Generic;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.ViewModels;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;

namespace TokenTracker.Views
{
    public partial class TokenListView : ContentPage
    {
        private enum ViewMode { Edit, View }

        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

        private ToolbarItem modeToggleItem = new ToolbarItem { Text = "Edit" };

        private ViewMode Mode { get; set; } = ViewMode.View;

        public TokenListView()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            TokenInfoService.StopTokenUpdates();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateModeToggleItem();
            UpdateForCurrentMode();
        }

        private void Handle_ModeToggleItem_Clicked(object sender, System.EventArgs e)
        {
            System.Console.WriteLine("CLICKED");

            Mode = (Mode == ViewMode.Edit) ? ViewMode.View : ViewMode.Edit;

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

            switch (Mode)
            {
                case ViewMode.Edit:
                    modeToggleItem = new ToolbarItem { Text = "Done" };
                    break;
                case ViewMode.View:
                    modeToggleItem = new ToolbarItem { Text = "Edit" };
                    break;
            }

            ToolbarItems.Add(modeToggleItem);
            modeToggleItem.Clicked += Handle_ModeToggleItem_Clicked;
        }

        private void UpdateForCurrentMode()
        {
            if (BindingContext is TokenListViewModel viewModel)
            {
                switch (Mode)
                {
                    case ViewMode.Edit:
                        if (viewModel.Tokens.Contains(Token.Dummy) == false)
                        {
                            viewModel.Tokens.Add(Token.Dummy);
                        }
                        TokenInfoService.StopTokenUpdates();
                        break;
                    case ViewMode.View:
                        viewModel.Tokens.Remove(Token.Dummy);
                        TokenInfoService.StartTokenUpdates();
                        break;
                }
            }
        }
    }
}
