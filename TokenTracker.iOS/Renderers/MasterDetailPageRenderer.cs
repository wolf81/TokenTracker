using System;
using TokenTracker.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MasterDetailPage), typeof(PhoneMasterDetailPageRenderer), UIKit.UIUserInterfaceIdiom.Phone)]
namespace TokenTracker.Renderers
{
    public class PhoneMasterDetailPageRenderer : PhoneMasterDetailRenderer
    {
        private MasterDetailPage MasterDetailPage => Element as MasterDetailPage;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && e.OldElement == null)
            {
                var masterDetailPage = e.NewElement as MasterDetailPage;
                masterDetailPage.IsPresentedChanged += Handle_MasterDetailPage_IsPresentedChanged;                
            }
            else if (e.OldElement != null && e.NewElement == null)
            {
                var masterDetailPage = e.OldElement as MasterDetailPage;
                masterDetailPage.IsPresentedChanged -= Handle_MasterDetailPage_IsPresentedChanged;
            }
        }

        private void Handle_MasterDetailPage_IsPresentedChanged(object sender, EventArgs e)
        {
            if (MasterDetailPage.IsPresented)
            {
                MasterDetailPage.Detail.FadeTo(0.4);
            }
            else
            {
                MasterDetailPage.Detail.FadeTo(1.0);
            }
        }
    }
}
