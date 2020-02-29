using System;

namespace Pathogen.Models
{
    public class ReportNotice
    {
        public int Confirmed { get; set; }
        public int Recovered { get; set; }
        public int Deaths { get; set; }
        public string CountryRegion { get; set; }
        public string ProvinceState { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Date { get; set; }
        
        public ReportNotice(string countryRegion, string provinceState,
            string date, double longitude, double latitude,
            int confirmed, int recovered, int deaths)
        {
            CountryRegion = countryRegion;
            ProvinceState = provinceState;
            Longitude = longitude;
            Latitude = latitude;
            Date = date;
            Confirmed = confirmed;
            Recovered = recovered;
            Deaths = deaths;
        }
    }
}
