using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Nito.Mvvm;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Pathogen.Models;
using Pathogen.Services;
using Pathogen.Views;
using Xamarin.Essentials;
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
        private List<string> _locations;
        private List<ReportNotice> _reportNotices;
        private List<NewsItem> _localNews;
        private List<NewsItem> _globalNews;
        private PlotModel _comparisonModel;
        private PlotModel _localTimeSeriesModel;

        public int CarouselPosition
        {
            get => _carouselPosition;
            set
            {
                _carouselPosition = value;
                OnPropertyChanged();
            }
        }

        public NotifyTask InitializeNotifier { get; private set; }

        public PlotModel ComparisonModel
        {
            get => _comparisonModel;
            set
            {
                _comparisonModel = value;
                OnPropertyChanged();
            }
        }

        public PlotModel LocalTimeSeriesModel
        {
            get => _localTimeSeriesModel;
            set
            {
                _localTimeSeriesModel = value;
                OnPropertyChanged();
            }
        }

        public string Location
        {
            get => _location;
            set
            {
                _location = value;

                Preferences.Set("Location", _location);

                LocalReport = (from report in _reportNotices
                               let locationString = report.ProvinceState != "" ?
                                   report.ProvinceState + ", " + report.CountryRegion : report.CountryRegion
                               where _location == locationString
                               orderby DateTime.Parse(report.Date) descending
                               select report).ToList()[0];

                //LocalNews = NotifyTask.Create(DataService.RetrieveLocalNews(_location), new List<NewsItem>());

                Task.Run(async () =>
                {
                    LocalNews = await DataService.RetrieveLocalNews(_location);
                });

                var points = from report in _reportNotices
                             let locationString = report.ProvinceState != "" ?
                                 report.ProvinceState + ", " + report.CountryRegion : report.CountryRegion
                             where locationString == _location
                             select new DataPoint(DateTimeAxis.ToDouble(DateTime.Parse(report.Date)), report.Confirmed);

                //Color primaryColor = (Color)Application.Current.Resources["PrimaryTextColor"];

                //OxyColor borderColor = OxyColor.FromRgb(Convert.ToByte(primaryColor.R), Convert.ToByte(primaryColor.G), Convert.ToByte(primaryColor.B));

                OxyColor borderColor = OxyColor.Parse(Application.Current.Resources["ModelBaseColor"].ToString());

                LocalTimeSeriesModel = new PlotModel
                {
                    TitleColor = borderColor,
                    PlotAreaBorderColor = borderColor,
                    TextColor = borderColor,
                    DefaultColors = OxyPalettes.Hot(8).Colors,
                    Title = _location + " Time Series",
                    Axes = {
                    new LinearAxis {
                        IsPanEnabled = false,
                        Position = AxisPosition.Left,
                        MinimumPadding = 0,
                        TicklineColor = borderColor
                    },
                    new DateTimeAxis {
                        Position = AxisPosition.Bottom,
                        MinimumPadding = 0,
                        TicklineColor = borderColor,
                        //MajorTickSize = 5,
                        //MinorTickSize = 5,
                        StringFormat = "MM/dd"
                    }
                },
                    Series = {
                    new LineSeries
                    {
                        Color = OxyColor.FromArgb(170, 128, 176, 128),
                        ItemsSource = points
                    }
                }
                };

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
                    Address = "Confirmed: " + _localReport.Confirmed.ToString() +
                              " Recovered: " + _localReport.Recovered.ToString() +
                              " Deaths: " + _localReport.Deaths.ToString(),
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

        public List<string> Locations
        {
            get => _locations;
            set
            {
                _locations = value;
                OnPropertyChanged();
            }
        }

        public List<ReportNotice> ReportNotices
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
                if (_pins.Count == 0)
                {
                    Hashtable pinLocations = new Hashtable();
                    foreach (var report in _reportNotices)
                    {
                        var locationString = report.ProvinceState != "" ?
                            report.ProvinceState + ", " + report.CountryRegion : report.CountryRegion;
                        if (!pinLocations.ContainsKey(locationString))
                        {
                            pinLocations.Add(locationString, true);
                            var pos = new Position(report.Latitude, report.Longitude);
                            _pins.Add(new Pin()
                            {
                                Position = pos,
                                Type = PinType.Generic,
                                Address = "Confirmed: " + report.Confirmed.ToString() +
                                          " Recovered: " + report.Recovered.ToString() +
                                          " Deaths: " + report.Deaths.ToString(),
                                Label = locationString
                            });
                        }
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

        public List<NewsItem> LocalNews
        {
            get => _localNews;
            set
            {
                _localNews = value;
                OnPropertyChanged();
            }
        }

        public List<NewsItem> GlobalNews
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
            InitializeNotifier = NotifyTask.Create(InitializeData());
        }

        public async Task InitializeData()
        {
            string storedLocation = Preferences.Get("Location", "Afghanistan");

            var GetGlobalNews = DataService.RetrieveGlobalNews();
            var GetLocations = DataService.RetrieveLocations();
            var GetReportNotices = DataService.RetrieveReportNotices();
            var GetLocalNews = DataService.RetrieveLocalNews(storedLocation);

            await Task.WhenAll(GetGlobalNews, GetLocations, GetReportNotices, GetLocalNews);

            LocalNews = GetLocalNews.Result;
            GlobalNews = GetGlobalNews.Result;
            ReportNotices = GetReportNotices.Result;
            Locations = GetLocations.Result;

            Console.WriteLine("LocalNews count: " + LocalNews.Count);
            Console.WriteLine("GlobalNews count: " + GlobalNews.Count);
            Console.WriteLine("ReportNotices count: " + ReportNotices.Count);
            Console.WriteLine("Locations count: " + Locations.Count);

            Location = storedLocation;

            var sortedPlaces = (from report in _reportNotices
                                let locationString = report.ProvinceState != "" ?
                                    report.ProvinceState + ", " + report.CountryRegion : report.CountryRegion
                                where report.Date == _reportNotices[0].Date
                                orderby report.Confirmed descending
                                select report).Take(4);

            var cols = from report in sortedPlaces
                       select new ColumnItem(report.Confirmed);

            ComparisonModel = new PlotModel
            {
                TitleColor = OxyColors.GhostWhite,
                PlotAreaBorderColor = OxyColors.GhostWhite,
                TextColor = OxyColors.GhostWhite,
                //DefaultColors = OxyPalettes.Cool(8).Colors,
                Title = "Total Confirmed Comparison",
                Axes = {
                    new LinearAxis {
                        IsPanEnabled = false,
                        Position = AxisPosition.Left,
                        MinimumPadding = 0,
                        TicklineColor = OxyColors.GhostWhite
                    },
                    new CategoryAxis {
                        IsPanEnabled = false,
                        Position = AxisPosition.Bottom,
                        MinimumPadding = 0,
                        TicklineColor = OxyColors.GhostWhite,
                        ItemsSource = from report in sortedPlaces
                                      let locationString = report.ProvinceState != "" ?
                                          report.ProvinceState + ", " + report.CountryRegion : report.CountryRegion
                                      select locationString
                    }
                },
                Series = {
                    new ColumnSeries
                    {
                        LabelPlacement = LabelPlacement.Middle,
                        FillColor = OxyColor.FromArgb(170, 128, 176, 128),
                        ItemsSource = cols
                    }
                }
            };
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
            if (InitializeNotifier.IsSuccessfullyCompleted)
            {
                foreach (var location in _locations)
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
