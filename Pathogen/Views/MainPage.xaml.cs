using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Pathogen.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            if (sender is CarouselView carouselView)
            {
                var grid = carouselView.Parent as Grid;
                var oldHeader = grid.Children.Where(c => Grid.GetRow(c) == 0 && Grid.GetColumn(c) == e.PreviousPosition);
                var newHeader = grid.Children.Where(c => Grid.GetRow(c) == 0 && Grid.GetColumn(c) == e.CurrentPosition);
                (oldHeader.ElementAt(0) as Button).SetDynamicResource(Label.TextColorProperty, "PrimaryTextColor");
                (newHeader.ElementAt(0) as Button).SetDynamicResource(Label.TextColorProperty, "HeaderColor");
            }
        }

        void OnHeaderClick(object sender, EventArgs args)
        {
            if (sender is Button button)
            {
                carouselView.Position = Grid.GetColumn(button);
            }
        }
    }
}
