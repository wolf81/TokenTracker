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

        #endregion
    }
}
