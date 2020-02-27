using System;
using System.Collections.Generic;
using Pathogen.ViewModels;
using Xamarin.Forms;

namespace Pathogen.Views
{
    public partial class NewsView : ContentView
    {
        public NewsView()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
    }
}
