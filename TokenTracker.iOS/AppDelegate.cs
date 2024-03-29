﻿using System;
using Foundation;
using Google.MobileAds;
using UIKit;

namespace TokenTracker.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            MobileAds.SharedInstance.RequestConfiguration.TestDeviceIdentifiers = new string[] { "3578f19d33ebf61c1a0b269b054560e1" };
            MobileAds.SharedInstance.Start((status) => { Console.WriteLine($"status: {status}"); });

            Firebase.Core.App.Configure();

            return base.FinishedLaunching(app, options);
        }
    }
}
