using Microcharts;
using SkiaSharp;
using Xamarin.Forms;

namespace TokenTracker.Controls
{
    public partial class TokenChartView : ContentView
    {
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
    }
}
