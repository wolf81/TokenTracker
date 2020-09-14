using System.Collections.Generic;
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

        private ToolbarItem rightNavigationItem;
        public ToolbarItem RightNavigationItem {
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
            rightButtonContainer.Children.Clear();

            if (RightNavigationItem is ToolbarItem item)
            {
                var button = new Button { ImageSource = item.IconImageSource, Text = item.Text };
                button.Clicked += Handle_RightNavigationButton_Clicked;
                button.Style = (Style)Application.Current.Resources["navigationButtonStyle"];                
                rightButtonContainer.Children.Add(button);
            }
        }

        private void Handle_RightNavigationButton_Clicked(object sender, System.EventArgs e)
        {
        }

        #endregion
    }
}
