using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Pathogen.Models;

namespace Pathogen.Services
{
    public class DataService
    {
        public static async Task<List<NewsItem>> RetrieveGlobalNews()
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

        public static async Task<List<NewsItem>> RetrieveLocalNews(string locale)
        {
            var newsApiClient = new NewsApiClient("0d8408e7585c4fa790539389d8d96fa6");

            var articlesResponse = await newsApiClient.GetEverythingAsync(new EverythingRequest
            {
                Q = "(Coronavirus OR COVID-19) AND " + locale,
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

        public static async Task<List<ReportNotice>> RetrieveReportNotices()
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

        public static async Task<List<string>> RetrieveLocations()
        {
            var connectionString = "mongodb+srv://pathogen:OdX2kR9DmzPLmuXk@corona-lvqsz.azure.mongodb.net/test?retryWrites=true&w=majority";

            const string databaseName = "corona";
            const string collectionName = "time_series";

            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);
            var collection = db.GetCollection<ReportNotice>(collectionName);

            var documents = await collection.Find<ReportNotice>(report => true).ToListAsync();

            var locs = new List<string>();

            foreach (ReportNotice report in documents)
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
    }
}
