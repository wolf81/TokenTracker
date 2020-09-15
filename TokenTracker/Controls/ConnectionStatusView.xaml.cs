using System;
using System.Threading.Tasks;
using TokenTracker.Extensions;
using TokenTracker.Services;
using Xamarin.Forms;

namespace TokenTracker.Controls
{
    public partial class ConnectionStatusView : ContentView
    {
        private Color fromColor;
        private Color toColor;

        public ConnectionState ConnectionState
        {
            get => (ConnectionState)GetValue(ConnectionStateProperty);
            set => SetValue(ConnectionStateProperty, value);
        } 

        public static readonly BindableProperty ConnectionStateProperty = BindableProperty.Create(nameof(ConnectionState), typeof(ConnectionState), typeof(ConnectionStatusView), ConnectionState.Disconnected, propertyChanged: Handle_PropertyChanged);

        public ConnectionStatusView()
        {
            InitializeComponent();

            Update();
        }

        protected override async void OnParentSet()
        {
            base.OnParentSet();

            if (Parent != null)
            {
                await AnimationLoop();
            }
            else
            {
                statusIcon.CancelHslColorAnimation();
            }
        }

        #region Private

        private async Task AnimationLoop()
        {
            while (true)
            {
                Action<Color> updateBackgroundColor = color => statusIcon.BackgroundColor = color;
                await Task.WhenAll(statusIcon.HslColorAnimation(fromColor, toColor, updateBackgroundColor, 1200, Easing.CubicIn));
                await Task.WhenAll(statusIcon.HslColorAnimation(toColor, fromColor, updateBackgroundColor, 1200, Easing.CubicOut));
            }
        }

        private static void Handle_PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ConnectionStatusView).Update();
        }

        private void Update()
        {
            switch (ConnectionState)
            {
                case ConnectionState.Busy:
                    fromColor = Color.Yellow;
                    toColor = Color.Yellow.WithLuminosity(0.3);
                    break;
                case ConnectionState.Connected:
                    fromColor = Color.Green;
                    toColor = Color.Green.WithLuminosity(0.6);
                    break;
                case ConnectionState.Disconnected:
                    fromColor = Color.Red;
                    toColor = Color.Red;
                    break;
            }
        }

        #endregion
    }
}
