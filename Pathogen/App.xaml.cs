using System;
using System.Threading.Tasks;
using Pathogen.Themes;
using Pathogen.Views;
using Xamarin.Forms;

namespace Pathogen
{
    public interface IEnvironment
    {
        Theme GetOperatingSystemTheme();
        Task<Theme> GetOperatingSystemThemeAsync();
    }

    public enum Theme { Light, Dark }

    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            base.OnStart();

            Theme theme = DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();

            SetTheme(theme);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            base.OnResume();

            Theme theme = DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();

            SetTheme(theme);
        }

        void SetTheme(Theme theme)
        {
            if (theme == Theme.Dark)
                Application.Current.Resources = new DarkTheme();
            else
                Application.Current.Resources = new LightTheme();
        }
    }
}
