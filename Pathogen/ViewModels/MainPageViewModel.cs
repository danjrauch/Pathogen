using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MongoDB.Driver;
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
        private NotifyTask<List<string>> locations;
        private NotifyTask<List<ReportNotice>> reportNotices;
        private NotifyTask<List<NewsItem>> localNews;
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

        public NotifyTask<List<string>> Locations
        {
            get => locations;
            set
            {
                locations = value;
                OnPropertyChanged();
            }
        }

        public NotifyTask<List<ReportNotice>> ReportNotices
        {
            get => reportNotices;
            set
            {
                reportNotices = value;
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

        public MainPageViewModel()
        {
            LocalNews = NotifyTask.Create(RetrieveLocalNews(), new List<NewsItem>());

            GlobalNews = NotifyTask.Create(RetrieveGlobalNews(), new List<NewsItem>());

            ReportNotices = NotifyTask.Create(RetrieveReportNotices(), new List<ReportNotice>());

            Locations = NotifyTask.Create(RetrieveLocations(), new List<string>());
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

        private async Task<List<ReportNotice>> RetrieveReportNotices()
        {
            var connectionString = "mongodb+srv://pathogen:OdX2kR9DmzPLmuXk@corona-lvqsz.azure.mongodb.net/test?retryWrites=true&w=majority";

            const string databaseName = "corona";
            const string collectionName = "time_series";

            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);
            var collection = db.GetCollection<ReportNotice>(collectionName);

            var documents = await collection.Find<ReportNotice>(report => true).ToListAsync();

            return documents;
        }

        private async Task<List<string>> RetrieveLocations()
        {
            var connectionString = "mongodb+srv://pathogen:OdX2kR9DmzPLmuXk@corona-lvqsz.azure.mongodb.net/test?retryWrites=true&w=majority";

            const string databaseName = "corona";
            const string collectionName = "time_series";

            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);
            var collection = db.GetCollection<ReportNotice>(collectionName);

            var documents = await collection.Find<ReportNotice>(report => true).ToListAsync();

            var locs = new List<string>();

            foreach(ReportNotice report in documents)
            {
                if (!locs.Contains(report.CountryRegion))
                    locs.Add(report.CountryRegion);
            }

            locs.Sort();

            return locs;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand PositionChangeCommand => new Xamarin.Forms.Command<string>((position) =>
        {
            //CarouselPosition = int.Parse(position);
        });

        public ICommand NavigateToNews => new Xamarin.Forms.Command(async (row) =>
        {
            if(row is NewsItem article)
            {
                var articlePage = new ArticlePage
                {
                    BindingContext = article
                };
                await Application.Current.MainPage.Navigation.PushAsync(articlePage);
            }
        });
    }
}
