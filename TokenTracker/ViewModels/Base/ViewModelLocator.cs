using System;
using System.Globalization;
using System.Reflection;
using TinyIoC;
using TokenTracker.Services;
using Xamarin.Forms;

namespace TokenTracker.ViewModels.Base
{
    public static class ViewModelLocator
    {
        private static readonly TinyIoCContainer _container;

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(AutoWireViewModelProperty, value);
        }

        public static bool UseMockService { get; set; }

        static ViewModelLocator()
        {
            _container = new TinyIoCContainer();

            // View models - by default, TinyIoC will register concrete classes as multi-instance.
            //_container.Register<LoginViewModel>();
            //_container.Register<CardsViewModel>();
            //_container.Register<CardPowerViewModel>();
            //_container.Register<CardConnectViewModel>();
            //_container.Register<MainMenuViewModel>();
            //_container.Register<SplashViewModel>();
            //_container.Register<PasswordRecoveryViewModel>();
            //_container.Register<ApproveCardViewModel>();
            //_container.Register<ManageCardViewModel>();
            //_container.Register<SyncCardViewModel>();

            // Services - by default, TinyIoC will register interface registrations as singletons.
            _container.Register<INavigationService, NavigationService>();
            _container.Register<ISettingsService, SettingsService>();
            _container.Register<ITokenInfoService, CoinCapTokenInfoService>();
        }

        public static void UpdateDependencies(bool useMockServices)
        {
            // Change injected dependencies
            if (useMockServices)
            {
                _container.Register<ITokenInfoService, MockTokenInfoService>();

                UseMockService = true;
            }
            else
            {
                _container.Register<ITokenInfoService, CoinCapTokenInfoService>();

                UseMockService = false;
            }
        }

        public static void RegisterSingleton<TInterface, T>() where TInterface : class where T : class, TInterface
        {
            _container.Register<TInterface, T>().AsSingleton();
        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Element view))
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }
            var viewModel = _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}
