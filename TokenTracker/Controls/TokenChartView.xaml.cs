using System.Collections.Generic;
using System.Windows.Input;
using Microcharts;
using SkiaSharp;
using TokenTracker.Models;
using TokenTracker.Services;
using Xamarin.Forms;

namespace TokenTracker.Controls
{
    public partial class TokenChartView : ContentView
    {
        //public IEnumerable<PricePoint> PriceHistory { get; set; }

        //public static readonly BindableProperty PriceHistoryProperty = BindableProperty.Create(nameo)

        // 1m 5m 1h 1d
        public ICommand ChangeIntervalCommand
        {
            get => (ICommand)GetValue(ChangeIntervalCommandProperty);
            set => SetValue(ChangeIntervalCommandProperty, value);
        }

        public static readonly BindableProperty ChangeIntervalCommandProperty = BindableProperty.Create(nameof(ChangeIntervalCommand), typeof(ICommand), typeof(TokenChartView), null, BindingMode.OneWay);

        public TokenChartView()
        {
            InitializeComponent();
        }        

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (Parent != null)
            {
                chartView.Chart = new LineChart
                {
                    Entries = new ChartEntry[]
                    {
                        new ChartEntry(0.5f) { Label = "12:35", ValueLabel = "0.5" },
                        new ChartEntry(0.6f) { },
                        new ChartEntry(0.4f) { Label = "12:45", ValueLabel = "0.4" },
                        new ChartEntry(0.8f) { },
                        new ChartEntry(0.9f) { Label = "12:55", ValueLabel = "0.9" },
                        new ChartEntry(0.7f) { },
                        new ChartEntry(1.1f) { Label = "13:05", ValueLabel = "1.1" },
                        new ChartEntry(1.2f) { },
                        new ChartEntry(1.3f) { Label = "13:15", ValueLabel = "1.3" },
                        new ChartEntry(1.1f) { },
                        new ChartEntry(1.2f) { Label = "13:25", ValueLabel = "1.2" },
                    },
                    LineMode = LineMode.Straight,
                    LabelOrientation = Orientation.Horizontal,
                    BackgroundColor = SKColor.Empty,                    
                };               
            }
        }

        #region Private

        private void Minute1_Button_Clicked(object sender, System.EventArgs e)
        {
            ChangeIntervalCommand?.Execute(Interval.Minute30);
        }

        private void Minute5_Button_Clicked(object sender, System.EventArgs e)
        {
            ChangeIntervalCommand?.Execute(Interval.Hour1);
        }

        private void Hour1_Button_Clicked(object sender, System.EventArgs e)
        {
            ChangeIntervalCommand?.Execute(Interval.Hour24);
        }

        private void Day1_Button_Clicked(object sender, System.EventArgs e)
        {
            ChangeIntervalCommand?.Execute(Interval.Day30);
        }

        #endregion
    }
}
