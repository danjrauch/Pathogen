using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Nito.Mvvm;
using Pathogen.Models;
using Pathogen.Services;
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
        private ObservableCollection<Pin> _pins = new ObservableCollection<Pin>();
        private string _location;
        private List<string> _locationSearchResults;
        private bool _showLocationSearchResults;
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

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                if (_reportNotices.IsSuccessfullyCompleted)
                {
                    foreach (ReportNotice report in _reportNotices.Result)
                    {
                        var locationString = report.ProvinceState != "" ?
                            report.ProvinceState + ", " + report.CountryRegion : report.CountryRegion;

                        if (_location == locationString)
                        {
                            LocalReport = report;
                        }
                    }
                }

                LocalNews = NotifyTask.Create(DataService.RetrieveLocalNews(_location), new List<NewsItem>());

                OnPropertyChanged();
            }
        }

        public List<string> LocationSearchResults
        {
            get => _locationSearchResults;
            set
            {
                _locationSearchResults = value;
                OnPropertyChanged();
            }
        }

        public bool ShowLocationSearchResults
        {
            get => _showLocationSearchResults;
            set
            {
                _showLocationSearchResults = value;
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
                LocalPins.Add(new Pin()
                {
                    Position = _localPosition,
                    Type = PinType.Generic,
                    Address = "Confirmed: " + _localReport.Confirmed.ToString(),
                    Label = locationString
                });
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

        public ObservableCollection<Pin> Pins
        {
            get
            {
                if (_reportNotices.IsSuccessfullyCompleted && _pins.Count == 0)
                {
                    foreach(var report in _reportNotices.Result)
                    {
                        var locationString = report.ProvinceState != "" ?
                            report.ProvinceState + ", " + report.CountryRegion : report.CountryRegion;
                        var pos = new Position(report.Latitude, report.Longitude);
                        _pins.Add(new Pin()
                        {
                            Position = pos,
                            Type = PinType.Generic,
                            Address = "Confirmed: " + report.Confirmed.ToString(),
                            Label = locationString
                        });
                    }
                }
                return _pins;
            }

            set
            {
                _pins = value;
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
            LocalNews = NotifyTask.Create(DataService.RetrieveLocalNews(_location), new List<NewsItem>());

            GlobalNews = NotifyTask.Create(DataService.RetrieveGlobalNews(), new List<NewsItem>());

            ReportNotices = NotifyTask.Create(DataService.RetrieveReportNotices(), new List<ReportNotice>());

            Locations = NotifyTask.Create(DataService.RetrieveLocations(), new List<string>());
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

        public ICommand PerformLocationSearch => new Command<string>((string query) =>
        {
            List<string> results = new List<string>();
            if (_locations.IsSuccessfullyCompleted)
            {
                foreach(var location in _locations.Result)
                {
                    if (location.ToLower().Contains(query.Trim().ToLower()))
                        results.Add(location);
                }
            }
            if (results.Count == 0)
                results.Add("No results");
            LocationSearchResults = results;
            ShowLocationSearchResults = true;
        });

        public ICommand ChangeLocation => new Xamarin.Forms.Command((row) =>
        {
            if (row is string location)
            {
                if (location != "No results")
                    Location = location;
                ShowLocationSearchResults = false;
            }
        });

        public ICommand NavigateToNews => new Xamarin.Forms.Command(async (row) =>
        {
            if (row is NewsItem article)
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
