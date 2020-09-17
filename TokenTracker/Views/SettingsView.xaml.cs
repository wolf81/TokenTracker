using TokenTracker.Services;
using TokenTracker.Utilities;
using TokenTracker.ViewModels;
using TokenTracker.ViewModels.Base;
using TokenTracker.Views.Base;

namespace TokenTracker.Views
{
    public partial class SettingsView : ContentPageBase, ITabbedViewAppearanceAware
    {
        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

        private SettingsViewModel ViewModel => BindingContext as SettingsViewModel;

        public SettingsView()
        {
            InitializeComponent();
        }

        #region ITabbedViewAppearanceAware

        public async void OnTabShown()
        {
            TokenInfoService.StopTokenUpdates();

            ViewModel.Update();

            await ViewModel.UpdateRatesAsync();
        }

        public void OnTabHidden()
        {
            if (TokenInfoService.IsConfigured)
            {
                TokenInfoService.StartTokenUpdates();
            }
        }

        #endregion
    }
}
