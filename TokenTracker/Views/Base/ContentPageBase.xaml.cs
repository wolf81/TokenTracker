using System;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;

namespace TokenTracker.Views.Base
{
    public partial class ContentPageBase : ContentPage
    {
        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

        private bool showConnectionStatusView = false;
        public bool ShowConnectionStatusView {
            get => showConnectionStatusView;
            set { showConnectionStatusView = value; OnPropertyChanged(nameof(ShowConnectionStatusView)); }
        }

        private NavigationMenuItem rightNavigationItem;
        public NavigationMenuItem RightNavigationItem {
            get => rightNavigationItem;
            set { rightNavigationItem = value; Update(); OnPropertyChanged(nameof(RightNavigationItem)); }
        }

        public ContentPageBase()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (TokenInfoService.IsConfigured)
            {
                TokenInfoService.StartTokenUpdates();
            }

            connectionStatusView.ConnectionState = TokenInfoService.State;

            TokenInfoService.ConnectionStateChanged += TokenInfoService_ConnectionStateChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            TokenInfoService.ConnectionStateChanged -= TokenInfoService_ConnectionStateChanged;
        }        

        #region Private

        private void TokenInfoService_ConnectionStateChanged(object sender, ConnectionState connectionState)
        {
            connectionStatusView.ConnectionState = connectionState;
        }

        private void Update()
        {
            foreach (var child in rightButtonContainer.Children)
            {
                if (child is Button button)
                {
                    button.Clicked -= Handle_RightNavigationButton_Clicked;
                }
            }

            rightButtonContainer.Children.Clear();

            if (RightNavigationItem is NavigationMenuItem item)
            {
                var button = new Button { ImageSource = item.IconImageSource, Text = item.Text };                
                button.Clicked += Handle_RightNavigationButton_Clicked;                
                button.Style = (Style)Application.Current.Resources["navigationButtonStyle"];
                rightButtonContainer.Children.Add(button);
            }
        }

        private void Handle_RightNavigationButton_Clicked(object sender, EventArgs e)
        {
            RightNavigationItem.Click();
        }

        #endregion
    }
}
