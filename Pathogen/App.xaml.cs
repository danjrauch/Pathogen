using System;
using System.Threading.Tasks;
using Pathogen.Themes;
using Pathogen.Views;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Pathogen
{
    //public interface IEnvironment
    //{
    //    Theme GetOperatingSystemTheme();
    //    Task<Theme> GetOperatingSystemThemeAsync();
    //}

    //public enum Theme { Light, Dark }

    public partial class App : Xamarin.Forms.Application
    {
        // TODO Connect to API or authenticate users
        public static string User = "DanRauch";

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            base.OnStart();

            //Theme theme = DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();
            AppTheme theme = AppInfo.RequestedTheme;

            SetTheme(theme);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            base.OnResume();

            //Theme theme = DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();
            AppTheme theme = AppInfo.RequestedTheme;

            SetTheme(theme);
        }

        void SetTheme(AppTheme theme)
        {
            if (theme == AppTheme.Dark)
                Application.Current.Resources = new DarkTheme();
            else
                Application.Current.Resources = new LightTheme();
        }
    }
}
