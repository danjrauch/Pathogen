using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pathogen.Models
{
    public class ReportNotice
    {
        [BsonId]  // _id
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("confirmed")]
        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int Confirmed { get; set; }
        [BsonElement("recovered")]
        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int Recovered { get; set; }
        [BsonElement("deaths")]
        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int Deaths { get; set; }
        [BsonElement("country/region")]
        public string CountryRegion { get; set; }
        [BsonElement("province/state")]
        public string ProvinceState { get; set; }
        [BsonElement("long")]
        public double Longitude { get; set; }
        [BsonElement("lat")]
        public double Latitude { get; set; }
        [BsonElement("date")]
        public string Date { get; set; }

        public ReportNotice()
        {
            CountryRegion = "";
            ProvinceState = "";
            Longitude = -1;
            Latitude = -1;
            Date = "";
            Confirmed = 0;
            Recovered = 0;
            Deaths = 0;
        }

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
