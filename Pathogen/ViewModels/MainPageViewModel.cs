using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Pathogen.Models;
using Xamarin.Forms;

namespace Pathogen.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private int carouselPosition;
        private string location;
        private List<string> locations;
        private List<NewsItem> localNews;
        private List<NewsItem> globalNews;

        public int CarouselPosition
        {
            get => carouselPosition;
            set
            {
                carouselPosition = value;
                OnPropertyChanged();
            }
        }

        public string Location
        {
            get => location;
            set
            {
                location = value;
                OnPropertyChanged();
            }
        }

        public List<string> Locations
        {
            get => locations;
            set
            {
                locations = value;
                OnPropertyChanged();
            }
        }

        public List<NewsItem> LocalNews
        {
            get => localNews;
            set
            {
                localNews = value;
                OnPropertyChanged();
            }
        }

        public List<NewsItem> GlobalNews
        {
            get => globalNews;
            set
            {
                globalNews = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            LocalNews = new List<NewsItem>() {
                new NewsItem("CNN", "Lorem ipsum something.", DateTime.Now),
                new NewsItem("New York Times", "Lorem ipsum something.", DateTime.Now),
                new NewsItem("Washington Post", "Lorem ipsum something.", DateTime.Now),
                new NewsItem("LA Times", "Lorem ipsum something.", DateTime.Now),
                new NewsItem("CNN", "Lorem ipsum something.", DateTime.Now),
                new NewsItem("New York Times", "Lorem ipsum something.", DateTime.Now),
                new NewsItem("CNN", "Lorem ipsum something.", DateTime.Now)
            };

            GlobalNews = new List<NewsItem>() {
                new NewsItem("LA Times", "Lorem ipsum something.", DateTime.Now),
                new NewsItem("CNN", "Lorem ipsum something.", DateTime.Now),
                new NewsItem("New York Times", "Lorem ipsum something.", DateTime.Now),
                new NewsItem("Washington Post", "Lorem ipsum something.", DateTime.Now),
                new NewsItem("CNN", "Lorem ipsum something.", DateTime.Now),
                new NewsItem("Washington Post", "Lorem ipsum something.", DateTime.Now),
                new NewsItem("New York Times", "Lorem ipsum something.", DateTime.Now)
            };

            Locations = new List<string>() {
                "Chicago, IL",
                "New York City, New York",
                "Madison, Wisconsin"
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand PositionChangeCommand => new Command<string>((position) =>
        {
            CarouselPosition = int.Parse(position);
        });
    }
}
