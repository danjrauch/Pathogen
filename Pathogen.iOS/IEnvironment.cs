﻿//using System;
//using UIKit;
//using Xamarin.Forms;
//using Pathogen;
//using Pathogen.iOS;
//using System.Threading.Tasks;
//using System.Linq;

//[assembly: Dependency(typeof(Environment_iOS))]
//namespace Pathogen.iOS
//{
//    public class Environment_iOS : IEnvironment
//    {
//        public Theme GetOperatingSystemTheme()
//        {
//            //Ensure the current device is running 12.0 or higher, because `TraitCollection.UserInterfaceStyle` was introduced in iOS 12.0
//            if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
//            {
//                var currentUIViewController = GetVisibleViewController();

//                var userInterfaceStyle = currentUIViewController.TraitCollection.UserInterfaceStyle;

//                switch (userInterfaceStyle)
//                {
//                    case UIUserInterfaceStyle.Light:
//                        return Theme.Light;
//                    case UIUserInterfaceStyle.Dark:
//                        return Theme.Dark;
//                    default:
//                        throw new NotSupportedException($"UIUserInterfaceStyle {userInterfaceStyle} not supported");
//                }
//            }
//            else
//            {
//                return Theme.Light;
//            }
//        }

//        // UIApplication.SharedApplication can only be referenced by the Main Thread, so we'll use Device.InvokeOnMainThreadAsync which was introduced in Xamarin.Forms v4.2.0
//        public Task<Theme> GetOperatingSystemThemeAsync() =>
//            Device.InvokeOnMainThreadAsync(GetOperatingSystemTheme);

//        static UIViewController GetVisibleViewController()
//        {
//            UIViewController viewController = null;

//            var window = UIApplication.SharedApplication.KeyWindow;

//            if (window.WindowLevel == UIWindowLevel.Normal)
//                viewController = window.RootViewController;

//            if (viewController is null)
//            {
//                window = UIApplication.SharedApplication
//                    .Windows
//                    .OrderByDescending(w => w.WindowLevel)
//                    .FirstOrDefault(w => w.RootViewController != null && w.WindowLevel == UIWindowLevel.Normal);

//                if (window is null)
//                    throw new InvalidOperationException("Could not find current view controller.");

//                viewController = window.RootViewController;
//            }

//            while (viewController.PresentedViewController != null)
//                viewController = viewController.PresentedViewController;

//            return viewController;
//        }
//    }
//}