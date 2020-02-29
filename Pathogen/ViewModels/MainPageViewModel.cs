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
using Pathogen.Views;
using Xamarin.Forms;

namespace Pathogen.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private int carouselPosition;
        private string location;
        private List<string> locations;
        private NotifyTask<List<NewsItem>> localNews;
        private NotifyTask<List<NewsItem>> globalNews;

        public INavigation Navigation { get; set; }

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

        public NotifyTask<List<NewsItem>> LocalNews
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

        public MainPageViewModel(INavigation navigation)
        {
            Navigation = navigation;

            Location = "Chicago";

            LocalNews = NotifyTask.Create(RetrieveLocalNews(), new List<NewsItem>());

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
                Q = "Coronavirus OR COVID-19",
                SortBy = SortBys.Popularity,
                Language = Languages.EN,
                From = DateTime.Now
            });

            if (articlesResponse.Status == Statuses.Ok)
            {
                return (from ar in articlesResponse.Articles
                        select new NewsItem(ar.Source.Name, ar.Title, ar.Description, ar.PublishedAt ?? DateTime.Now))
                        .ToList();
            }
            else
            {
                return new List<NewsItem>();
            }
        }

        private async Task<List<NewsItem>> RetrieveLocalNews()
        {
            var newsApiClient = new NewsApiClient("0d8408e7585c4fa790539389d8d96fa6");

            var articlesResponse = await newsApiClient.GetTopHeadlinesAsync(new TopHeadlinesRequest
            {
                Q = "Coronavirus OR COVID-19",
                Country = Countries.US,
                Language = Languages.EN,
            });

            if (articlesResponse.Status == Statuses.Ok)
            {
                return (from ar in articlesResponse.Articles
                        select new NewsItem(ar.Source.Name, ar.Title, ar.Description, ar.PublishedAt ?? DateTime.Now))
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

        public ICommand NavigateToNews => new Command(async (row) =>
        {
            if(row is NewsItem article)
            {
                Console.WriteLine(article.Publication);
                var articlePage = new ArticlePage();
                articlePage.BindingContext = article;
                await Navigation.PushAsync(articlePage);
            }
        });
    }
}
