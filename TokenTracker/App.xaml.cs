using System.Threading.Tasks;
using TokenTracker.Services;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;

namespace TokenTracker
{
    public partial class App : Application
    {
        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();
        private ISettingsService SettingsService => ViewModelLocator.Resolve<ISettingsService>();
        private ICache Cache => ViewModelLocator.Resolve<ICache>();

        public App()
        {
            InitializeComponent();

            InitApp();
        }

        private void InitApp()
        {
            if (SettingsService.IsFirstRun == true)
            {
                Cache.Configure();
                SettingsService.IsFirstRun = false;
            }

            ViewModelLocator.UpdateDependencies(useMockServices: SettingsService.UseMocks);
        }

        private Task InitNavigation()
        {
            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            return navigationService.InitializeAsync();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await InitNavigation();            

            base.OnResume();
        }

        protected override void OnSleep()
        {
            TokenInfoService.StopTokenUpdates();
        }
    }
}
