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

            var steps = Math.Min(PricePoints.Count(), 30);
            PricePoint[] pricePoints;

            if (PricePoints.Count() > 10)
            {
                pricePoints = PricePoints.Where((v, idx) => idx / steps == 0).ToArray();
            }
            else
            {
                pricePoints = PricePoints.ToArray();
            }

            var firstPrice = pricePoints.First();
            var minValue = (double)(firstPrice?.PriceUSD ?? 0) * 0.95;
            var maxValue = (double)(firstPrice?.PriceUSD ?? 0) * 1.05;

            for (var i = 0; i < pricePoints.Length; i++)
            {
                var price = (int)minValue > 1.0 ? (int)pricePoints[i].PriceUSD : NormalizedPrice((double)pricePoints[i].PriceUSD, 2);
                if (price < minValue) { minValue = price; }
                if (price > maxValue) { maxValue = price; }

                var entry = new ChartEntry((float)price) { Label = $"{i}", ValueLabel = $"{price}" };
                entries.Add(entry);
            }

            chartView.Chart = new LineChart
            {
                Entries = entries,
                MinValue = (float)minValue,
                MaxValue = (float)maxValue,
                LineMode = LineMode.Straight,
                LabelOrientation = Orientation.Horizontal,
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
            ChangeIntervalCommand?.Execute(Interval.Day1);
        }

        private void Week1_Button_Clicked(object sender, EventArgs e)
        {
            ChangeIntervalCommand?.Execute(Interval.Week1);
        }

        private void Month1_Button_Clicked(object sender, EventArgs e)
        {
            ChangeIntervalCommand?.Execute(Interval.Month1);
        }

        private void Year1_Button_Clicked(object sender, EventArgs e)
        {
            ChangeIntervalCommand?.Execute(Interval.Year1);
        }

        #endregion
    }
}
