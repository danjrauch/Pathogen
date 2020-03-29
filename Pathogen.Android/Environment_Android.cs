//using System;
//using System.Threading.Tasks;
//using Android.Content.Res;
//using Plugin.CurrentActivity;
//using Xamarin.Forms;
//using Android.OS;
//using Pathogen.Droid;


//[assembly: Dependency(typeof(Environment_Android))]
//namespace Pathogen.Droid
//{
//    public class Environment_Android : IEnvironment
//    {
//        public Task<Theme> GetOperatingSystemThemeAsync() =>
//            Task.FromResult(GetOperatingSystemTheme());

//        public Theme GetOperatingSystemTheme()
//        {
//            //Ensure the device is running Android Froyo or higher because UIMode was added in Android Froyo, API 8.0
//            if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
//            {
//                var uiModelFlags = CrossCurrentActivity.Current.AppContext.Resources.Configuration.UiMode & UiMode.NightMask;

//                switch (uiModelFlags)
//                {
//                    case UiMode.NightYes:
//                        return Theme.Dark;

//                    case UiMode.NightNo:
//                        return Theme.Light;

//                    default:
//                        throw new NotSupportedException($"UiMode {uiModelFlags} not supported");
//                }
//            }
//            else
//            {
//                return Theme.Light;
//            }
//        }
//    }
//}
