using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Nito.Mvvm;
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
        private NotifyTask<List<NewsItem>> globalNews;

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

        public NotifyTask<List<NewsItem>> GlobalNews
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

            GlobalNews = NotifyTask.Create(RetrieveGlobalNews(), new List<NewsItem>());

            Locations = new List<string>() {
                "Chicago, IL",
                "New York City, New York",
                "Madison, Wisconsin"
            };
        }

        private async Task<List<NewsItem>> RetrieveGlobalNews()
        {
            var newsApiClient = new NewsApiClient("0d8408e7585c4fa790539389d8d96fa6");

            var articlesResponse = await newsApiClient.GetEverythingAsync(new EverythingRequest
            {
                Q = "Coronavirus",
                SortBy = SortBys.Popularity,
                Language = Languages.EN,
                From = DateTime.Now
            });

            if (articlesResponse.Status == Statuses.Ok)
            {
                return (from ar in articlesResponse.Articles
                        select new NewsItem(ar.Author, ar.Description, ar.PublishedAt ?? DateTime.Now))
                        .ToList();
            }
            else
            {
                return new List<NewsItem>();
            }
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
