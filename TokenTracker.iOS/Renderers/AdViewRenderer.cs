using System;
using CoreGraphics;
using Google.MobileAds;
using TokenTracker.Controls;
using TokenTracker.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AdView), typeof(AdViewRenderer))]
namespace TokenTracker.Renderers
{
    public class AdViewRenderer : ViewRenderer<AdView, BannerView>
    {
        private const string BANNER_ID = "ca-app-pub-3940256099942544/2934735716"; // "ca-app-pub-9770292984772276/8518544922";

        private BannerView adView;

        private BannerView CreateNativeAdControl()
        {
            if (adView != null)
            {
                return adView;
            }

            // Setup your BannerView, review AdSizeCons class for more Ad sizes. 
            adView = new BannerView(size: AdSizeCons.SmartBannerPortrait, origin: new CGPoint(0, UIScreen.MainScreen.Bounds.Size.Height - AdSizeCons.Banner.Size.Height))
            {
                RootViewController = GetVisibleViewController()
            };
            adView.AdUnitId = BANNER_ID;

            // Wire AdReceived event to know when the Ad is ready to be displayed
            adView.AdReceived += (object sender, EventArgs e) =>
            {
                //ad has come in                
            };

            adView.LoadRequest(GetRequest());
            return adView;
        }

        private Request GetRequest()
        {
            var request = Request.GetDefaultRequest();
            // Requests test ads on devices you specify. Your test device ID is printed to the console when
            // an ad request is made. GADBannerView automatically returns test ads when running on a
            // simulator. After you get your device ID, add it here
            //request.TestDevices = new [] { Request.SimulatorId.ToString () };
            return request;
        }

        /// 
        /// Gets the visible view controller.
        /// 
        /// The visible view controller.
        private UIViewController GetVisibleViewController()
        {
            var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (rootController.PresentedViewController == null)
            {
                return rootController;
            }

            if (rootController.PresentedViewController is UINavigationController navigationController)
            {
                return navigationController.VisibleViewController;
            }

            if (rootController.PresentedViewController is UITabBarController tabBarController)
            {
                return tabBarController.SelectedViewController;
            }

            return rootController.PresentedViewController;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AdView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                CreateNativeAdControl();
                SetNativeControl(adView);
            }
        }
    }
}
