using System;
using System.Threading.Tasks;
using TokenTracker.Extensions;
using Xamarin.Forms;

namespace TokenTracker.Controls
{
    public partial class ConnectionStatusView : ContentView
    {
        public ConnectionStatusView()
        {
            InitializeComponent();
        }

        protected override async void OnParentSet()
        {
            base.OnParentSet();

            if (Parent != null)
            {
                await AnimationLoop();
                //statusIcon.ColorTo
            }
            else
            {

            }
        }


        async Task AnimationLoop()
        {
            while (true)
            {
                Action<Color> textCallback = color => statusIcon.BackgroundColor = color;
                await Task.WhenAll(
                    statusIcon.HslColorAnimation(Color.LightGreen, Color.Green, textCallback, 1200, Easing.CubicInOut));
                await Task.WhenAll(
                    statusIcon.HslColorAnimation(Color.Green, Color.LightGreen, textCallback, 1200, Easing.CubicInOut));
            }
        }
    }
}
