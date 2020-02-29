using System;
using System.Collections.Generic;
using Pathogen.ViewModels;
using Xamarin.Forms;

namespace Pathogen.Views
{
    public partial class ArticlePage : ContentPage
    {
        public ArticlePage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(Navigation);
        }
    }
}
