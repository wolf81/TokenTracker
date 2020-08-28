using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
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
                var masterPage = Application.Current.MainPage as MasterDetailView;
                var mainPage = masterPage.Detail;
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
            var mainPage = Application.Current.MainPage as CustomNavigationView;

            if (mainPage != null)
            {
                mainPage.Navigation.RemovePage(
                    mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        public Task RemoveBackStackAsync()
        {
            var masterPage = Application.Current.MainPage as MasterDetailView;
            if (masterPage.Detail is CustomNavigationView mainPage)
            {
                for (int i = 0; i < mainPage.Navigation.NavigationStack.Count - 1; i++)
                {
                    var page = mainPage.Navigation.NavigationStack[i];
                    mainPage.Navigation.RemovePage(page);
                }
            }

            return Task.FromResult(true);
        }

        private async Task InternalNavigateToAsync(Type viewModelType, Dictionary<string, object> parameter)
        {
            var page = CreatePage(viewModelType);

            if (Application.Current.MainPage == null)
            {
                var mainPage = CreatePage(typeof(MasterDetailViewModel)) as MasterDetailPage;
                Application.Current.MainPage = mainPage;
            }

            var masterPage = Application.Current.MainPage as MasterDetailView;
            if (masterPage.Detail is CustomNavigationView navigationPage)
            {
                await navigationPage.PushAsync(page);
            }
            else
            {
                masterPage.Detail = new CustomNavigationView(page);
            }

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
    }
}
