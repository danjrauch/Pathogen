using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Xamarin.Forms.Maps;

namespace Pathogen.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private int _carouselPosition;
        private ReportNotice _localReport;
        private Position _localPosition;
        private ObservableCollection<Pin> _localPins = new ObservableCollection<Pin>();
        private string _location;
        private NotifyTask<List<string>> _locations;
        private NotifyTask<List<ReportNotice>> _reportNotices;
        private NotifyTask<List<NewsItem>> _localNews;
        private NotifyTask<List<NewsItem>> _globalNews;

        public int CarouselPosition
        {
            get => _carouselPosition;
            set
            {
                _carouselPosition = value;
                OnPropertyChanged();
            }
        }

        public ReportNotice LocalReport
        {
            get => _localReport;
            set
            {
                _localReport = value;
                var locationString = _localReport.ProvinceState != "" ?
                            _localReport.ProvinceState + ", " + _localReport.CountryRegion : _localReport.CountryRegion;
                LocalPosition = new Position(_localReport.Latitude, _localReport.Longitude);
                LocalPins.Add(new Pin() { Position = _localPosition, Type = PinType.Generic, Label = locationString });
                OnPropertyChanged();
            }
        }

        public Position LocalPosition
        {
            get => _localPosition;
            set
            {
                _localPosition = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Pin> LocalPins
        {
            get => _localPins;
            set
            {
                _localPins = value;
                OnPropertyChanged();
            }
        }

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                if(_reportNotices.IsSuccessfullyCompleted)
                {
                    foreach(ReportNotice report in _reportNotices.Result)
                    {
                        var locationString = report.ProvinceState != "" ?
                            report.ProvinceState + ", " + report.CountryRegion : report.CountryRegion;

                        if(_location == locationString)
                        {
                           LocalReport = report;
                        }
                    }
                }
                OnPropertyChanged();
            }
        }

        public NotifyTask<List<string>> Locations
        {
            get => _locations;
            set
            {
                _locations = value;
                OnPropertyChanged();
            }
        }

        public NotifyTask<List<ReportNotice>> ReportNotices
        {
            get => _reportNotices;
            set
            {
                _reportNotices = value;
                OnPropertyChanged();
            }
        }

        public NotifyTask<List<NewsItem>> LocalNews
        {
            get => _localNews;
            set
            {
                _localNews = value;
                OnPropertyChanged();
            }
        }

        public NotifyTask<List<NewsItem>> GlobalNews
        {
            get => _globalNews;
            set
            {
                _globalNews = value;
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

            documents = documents.OrderByDescending(report => DateTime.Parse(report.Date)).ToList();

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
                var locationString = report.ProvinceState != "" ?
                    report.ProvinceState + ", " + report.CountryRegion : report.CountryRegion;

                if (!locs.Contains(locationString))
                {
                    locs.Add(locationString);
                }
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
            CarouselPosition = int.Parse(position);
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
