using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using TokenTracker.Utilities;
using TokenTracker.ViewModels;
using TokenTracker.ViewModels.Base;
using TokenTracker.Views;
using Xamarin.Forms;

namespace TokenTracker.Services
{
    public class NavigationService : INavigationService
    {
        public ViewModelBase PreviousPageViewModel
        {
            get
            {
                var masterPage = Application.Current.MainPage as TabbedView;                
                var mainPage = masterPage.CurrentPage;
                var viewModel = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2].BindingContext;
                return viewModel as ViewModelBase;
            }
        }

        public Task InitializeAsync()
        {
            return NavigateToAsync<TokenStatusViewModel>();
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(Dictionary<string, object> parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task RemoveLastFromBackStackAsync()
        {
            if (Application.Current.MainPage is CustomNavigationView mainPage)
            {
                mainPage.Navigation.RemovePage(
                    mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        public Task RemoveBackStackAsync()
        {
            var masterPage = Application.Current.MainPage as TabbedView;
            if (masterPage.CurrentPage is CustomNavigationView mainPage)
            {
                for (int i = 0; i < mainPage.Navigation.NavigationStack.Count - 1; i++)
                {
                    var page = mainPage.Navigation.NavigationStack[i];
                    mainPage.Navigation.RemovePage(page);
                }
            }

            return Task.FromResult(true);
        }

        #region Private

        private TabbedView GetTabbedView()
        {
            if (Application.Current.MainPage == null)
            {
                var tabbedView = new TabbedView();
                Application.Current.MainPage = tabbedView;
                tabbedView.CurrentPageChanged += Handle_TabbedView_CurrentPageChanged;

                var statusView = CreatePage(typeof(TokenStatusView));
                tabbedView.Children.Add(new CustomNavigationView(statusView));

                var walletView = CreatePage(typeof(TokenWalletView));
                tabbedView.Children.Add(new CustomNavigationView(walletView));

                var settingsView = CreatePage(typeof(SettingsView));
                tabbedView.Children.Add(new CustomNavigationView(settingsView));
            }

            return Application.Current.MainPage as TabbedView;
        }

        private void Handle_TabbedView_CurrentPageChanged(object sender, EventArgs e)
        {
            var tabbedView = GetTabbedView();

            if (tabbedView is ITabbedViewAppearanceAware tabbedViewAppearanceAware)
            {
                tabbedViewAppearanceAware.TabSelected();
            }
        }

        private async Task InternalNavigateToAsync(Type viewModelType, Dictionary<string, object> parameter)
        {
            var masterPage = GetTabbedView();

            foreach (var childPage in masterPage.Children)
            {
                if (childPage is NavigationPage navPage)
                {
                    if (navPage.RootPage.BindingContext.GetType() == viewModelType) {
                        masterPage.SelectedItem = navPage;
                        await (navPage.RootPage.BindingContext as ViewModelBase).InitializeAsync(parameter);
                        return;
                    }
                }
            }

            var page = CreatePage(viewModelType);
            await (masterPage.CurrentPage as CustomNavigationView).PushAsync(page);
            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private Page CreatePage(Type viewModelType)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = null;
            try
            {
                page = Activator.CreateInstance(pageType) as Page;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return page;
        }

        #endregion
    }
}
