using TokenTracker.ViewModels;
using TokenTracker.Views.Base;

namespace TokenTracker.Views
{
    public partial class SettingsView : ContentPageBase
    {
        private SettingsViewModel ViewModel => BindingContext as SettingsViewModel;

        public SettingsView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.Update();

            await ViewModel.UpdateRatesAsync();
        }
    }
}
