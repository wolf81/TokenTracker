using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microcharts;
using SkiaSharp;
using TokenTracker.Extensions;
using TokenTracker.Models;
using TokenTracker.Services;
using Xamarin.Forms;

namespace TokenTracker.Controls
{
    public partial class TokenChartView : ContentView
    {
        private Interval interval = Interval.Day1;

        public IEnumerable<PricePoint> PricePoints
        {
            get => (IEnumerable<PricePoint>)GetValue(PricePointsProperty);
            set => SetValue(PricePointsProperty, value);
        }

        public static readonly BindableProperty PricePointsProperty = BindableProperty.Create(nameof(PricePointsProperty), typeof(IEnumerable<PricePoint>), typeof(TokenChartView), null, propertyChanged: Handle_PropertyChanged);

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

        private void Update()
        {
            var entries = new List<ChartEntry> { };
            int steps = 0;

            switch (interval)
            {
                case Interval.Year1: steps = 30; break;
                default: steps = 1; break;
            }

            var pricePoints = PricePoints.Where((v, idx) => idx % steps == 0).ToArray();
            var firstPrice = pricePoints.First();
            var minValue = (double)firstPrice?.PriceUSD;
            var maxValue = (double)firstPrice?.PriceUSD;

            for (var i = 0; i < pricePoints.Length; i++)
            {
                var price = (int)minValue > 1.0 ? (int)pricePoints[i].PriceUSD : NormalizedPrice((double)pricePoints[i].PriceUSD, 2);
                if (price < minValue) { minValue = price; }
                if (price > maxValue) { maxValue = price; }

                string label = null;
                var valueLabel = $"{price}";    
                var time = DateTimeOffset.FromUnixTimeMilliseconds(pricePoints[i].Time).DateTime;

                switch (interval)
                {
                    case Interval.Day1:
                        label = i % 2 == 0 ? time.ToString("HH:mm") : null;
                        if (i % 2 != 0) { valueLabel = null; }
                        break;
                    case Interval.Week1:
                        label = time.ToString("ddd");
                        break;
                    case Interval.Month1:
                        //var labelIndexes = new List<int> { 1, 6, 11, 16, 21, 26, 31 };
                        label = i % 5 == 0 ? $"{time.Day}" : null;
                        if (i % 5 != 0) { valueLabel = null; }
                        break;
                    case Interval.Year1:                        
                        label = time.ToString("MMM");
                        break;
                }

                var entry = new ChartEntry((float)price) { Label = label, ValueLabel = valueLabel };
                entries.Add(entry);
            }

            chartView.Chart = new LineChart
            {
                Entries = entries,
                MinValue = (float)(minValue * 0.95),
                MaxValue = (float)(maxValue * 1.05),
                LineMode = LineMode.Spline,
                LabelOrientation = Orientation.Default,
                BackgroundColor = SKColor.Empty,
            };
        }

        private static double NormalizedPrice(double value, int numSignificantDigits)
        {
            if (value >= 1.0)
            {
                return Math.Round(value, numSignificantDigits);
            }
            else
            {
                return value.RoundToSignificantDigits(numSignificantDigits);
            }
        }

        private static void Handle_PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TokenChartView).Update();
        }

        private void Day1_Button_Clicked(object sender, EventArgs e)
        {
            interval = Interval.Day1;
            OnIntervalChanged();
        }

        private void Week1_Button_Clicked(object sender, EventArgs e)
        {
            interval = Interval.Week1;
            OnIntervalChanged();
        }

        private void Month1_Button_Clicked(object sender, EventArgs e)
        {
            interval = Interval.Month1;
            OnIntervalChanged();
        }

        private void Year1_Button_Clicked(object sender, EventArgs e)
        {
            interval = Interval.Year1;
            OnIntervalChanged();
        }

        private void OnIntervalChanged()
        {
            ChangeIntervalCommand?.Execute(interval);
        }

        #endregion
    }
}
