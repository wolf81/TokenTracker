using TokenTracker.Services;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;

namespace TokenTracker.Views.Base
{
    public partial class ContentPageBase : ContentPage
    {
        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

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
        }
    }
}
