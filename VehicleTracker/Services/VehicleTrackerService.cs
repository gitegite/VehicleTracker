using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VehicleTracker.Data;
using VehicleTracker.Models;

namespace VehicleTracker.Services
{
    public class VehicleTrackerService : IVehicleTrackerService
    {
        private readonly IVehicleTrackerContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public VehicleTrackerService(IVehicleTrackerContext context, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _context = context;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public async Task<Vehicle> GetVehicleById(Guid id)
        {
            return await _context.Vehicle.Include(v => v.Locations).SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task RegisterVehicle(Vehicle vehicle)
        {
            _context.Vehicle.Add(vehicle);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateVehicle(Vehicle vehicle)
        {
            _context.Entry(vehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveVehicle(Vehicle vehicle)
        {
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Location>> GetVehicleLocationByDate(Guid id, DateTime from, DateTime to)
        {
            return await _context.Location
                        .Where(l => l.TimeOfRecord >= from && l.TimeOfRecord >= to && l.VehicleId == id)
                        .ToArrayAsync();
        }

        public async Task<Location> GetCurrentVehicleLocation(Guid id)
        {
            return await _context.Location
                        .Where(l => l.VehicleId == id)
                        .OrderByDescending(l => l.TimeOfRecord)
                        .FirstOrDefaultAsync();
        }

        private async Task<string> GetRealAddress(decimal latitude = 40.714224m, decimal longitude = -73.961452m)
        {
            var apiKey = _configuration["GoogleAPIKey"];
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={apiKey}");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeAnonymousType(jsonResponse, new
                {
                    results = new[]
                    {
                        new
                        {
                            formatted_address = ""
                        }
                    }
                });
                // Google's API will return the location from most specific to least specific
                // see https://developers.google.com/maps/documentation/geocoding/intro#ComponentFiltering
                //"formatted_address" : "277 Bedford Avenue, Brooklyn, NY 11211, USA",
                //"formatted_address" : "New York, USA",
                //"formatted_address" : "United States",
                return results.results.FirstOrDefault()?.formatted_address;
            }
            else
            {
                return "Location not found by Google Map's API";
            }
        }

        public async Task RecordCurrentLocation(Guid vehicleId, Location location)
        {
            location.VehicleId = vehicleId;
            location.TimeOfRecord = DateTime.UtcNow;
            location.RealAddress = await GetRealAddress(location.Latitude, location.Longitude);
            _context.Location.Add(location);
            await _context.SaveChangesAsync();
        }

        public async Task<Vehicle> GetVehicleByUser(string userId)
        {
            return await _context.Vehicle.SingleOrDefaultAsync(v => v.UserId == userId);
        }
    }
}
