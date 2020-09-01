using Microcharts;
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
                    new ChartEntry(0.5f) { Label = "1" },
                    new ChartEntry(0.6f) { Label = "2" },
                    new ChartEntry(0.4f) { Label = "3" },
                    new ChartEntry(0.7f) { Label = "4" },
                    new ChartEntry(0.8f) { Label = "5" },
                    new ChartEntry(0.7f) { Label = "6" },
                    new ChartEntry(0.9f) { Label = "7" },
                    new ChartEntry(1.1f) { Label = "8" },
                    }
                };
            }
        }
    }
}
