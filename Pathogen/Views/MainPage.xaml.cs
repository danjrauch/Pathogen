using System;
using System.Collections.Generic;
using System.Linq;
using Pathogen.ViewModels;
using Xamarin.Forms;

namespace Pathogen.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(Navigation);
        }
    }
}
