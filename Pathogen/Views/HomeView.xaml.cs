using System;
using System.Collections.Generic;
using Pathogen.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace Pathogen.Views
{
    public partial class HomeView : ContentView
    {
        private static readonly SKColor labelColor = ((Color) Application.Current.Resources["PrimaryTextColor"]).ToSKColor();
        private static readonly SKColor dataPointColor = ((Color)Application.Current.Resources["SecondaryTextColor"]).ToSKColor();

        private readonly List<Microcharts.Entry> _entries = new List<Microcharts.Entry>()
        {
            new Microcharts.Entry(200)
            {
                TextColor = labelColor,
                Label = "January",
                ValueLabel = "200",
                Color = dataPointColor
            },
            new Microcharts.Entry(300)
            {
                TextColor = labelColor,
                Label = "February",
                ValueLabel = "300",
                Color = dataPointColor
            },
            new Microcharts.Entry(305)
            {
                TextColor = labelColor,
                Label = "March",
                ValueLabel = "305",
                Color = dataPointColor
            },
            new Microcharts.Entry(320)
            {
                TextColor = labelColor,
                Label = "April",
                ValueLabel = "320",
                Color = dataPointColor
            },
            new Microcharts.Entry(330)
            {
                TextColor = labelColor,
                Label = "May",
                ValueLabel = "330",
                Color = dataPointColor
            },
            new Microcharts.Entry(340)
            {
                TextColor = labelColor,
                Label = "June",
                ValueLabel = "340",
                Color = dataPointColor
            },
            new Microcharts.Entry(350)
            {
                TextColor = labelColor,
                Label = "July",
                ValueLabel = "350",
                Color = dataPointColor
            },
            new Microcharts.Entry(400)
            {
                TextColor = labelColor,
                Label = "August",
                ValueLabel = "400",
                Color = dataPointColor
            },
        };

        public HomeView()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();

            MyChart.Chart = new Microcharts.BarChart
            {
                Entries = _entries,
                BackgroundColor = SKColors.Transparent,
                LabelTextSize = 20
            };
        }
    }
}
