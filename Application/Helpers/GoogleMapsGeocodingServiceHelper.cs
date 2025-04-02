using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Application.Services;

namespace Application.Helpers
{
    public class GoogleMapsGeocodingServiceHelper
    {
        private readonly RestClientService _restClientService;
        private readonly string _apiKey;

        public GoogleMapsGeocodingServiceHelper(RestClientService restClientService, IConfiguration configuration)
        {
            _restClientService = restClientService;
            _apiKey = configuration["GoogleMaps:ApiKey"];
        }

        public async Task<(double? Latitude, double? Longitude)> GetLatLongFromAddressAsync(string address)
        {
            var endpoint = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";

            var response = await _restClientService.GetAsync<GoogleGeocodeResponse>(endpoint);

            if (response.IsSuccessful && response.Data?.Results?.Length > 0)
            {
                var location = response.Data.Results[0].Geometry.Location;
                return (location.Lat, location.Lng);
            }

            return (null, null); // Return null if geocoding fails
        }
    }

    // Google Geocoding response classes remain the same
    public class GoogleGeocodeResponse
    {
        public GeocodeResult[] Results { get; set; }
    }

    public class GeocodeResult
    {
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class GoogleMapsSettings
    {
        public string ApiKey { get; set; }
    }
}
