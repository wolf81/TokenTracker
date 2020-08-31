using Android.Content;
using Android.Widget;
using TokenTracker.Controls;
using TokenTracker.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using BannerView = Android.Gms.Ads.AdView;

[assembly: ExportRenderer(typeof(AdView), typeof(AdViewRenderer))]
namespace TokenTracker.Renderers
{
    public class AdViewRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<AdView, BannerView>
    {
        private const string BANNER_ID = AdView.TEST_BANNER_ID; // "ca-app-pub-9770292984772276/3733122101";

        public AdViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<AdView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control == null)
            {
                SetNativeControl(CreateAdView());
            }
        }

        #region Private

        private BannerView CreateAdView()
		{
			var adView = new BannerView(Context)
			{
				AdSize = Android.Gms.Ads.AdSize.Banner,
				AdUnitId = BANNER_ID
			};

			adView.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
			adView.LoadAd(new Android.Gms.Ads.AdRequest.Builder().Build());

			return adView;
		}

        #endregion
    }
}
