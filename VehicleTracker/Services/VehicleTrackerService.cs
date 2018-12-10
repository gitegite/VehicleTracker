using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracker.Data;
using VehicleTracker.Models;

namespace VehicleTracker.Services
{
    public class VehicleTrackerService : IVehicleTrackerService
    {
        private readonly IVehicleTrackerContext _context;

        public VehicleTrackerService(IVehicleTrackerContext context)
        {
            _context = context;
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
                        .Select(l => new Location
                        {
                            Id = l.Id,
                            Latitude = l.Latitude,
                            Longitude = l.Longitude,
                            TimeOfRecord = l.TimeOfRecord
                        })
                        .ToArrayAsync();
        }

        public async Task<Location> GetCurrentVehicleLocation(Guid id)
        {
            return await _context.Location
                        .Where(l => l.VehicleId == id)
                        .OrderByDescending(l => l.TimeOfRecord)
                        .FirstOrDefaultAsync();
        }

        public async Task RecordCurrentLocation(Guid vehicleId, Location location)
        {
            location.VehicleId = vehicleId;
            location.TimeOfRecord = DateTime.UtcNow;
            _context.Location.Add(location);
            await _context.SaveChangesAsync();
        }

        public async Task<Vehicle> GetVehicleByUser(string userId)
        {
            return await _context.Vehicle.SingleOrDefaultAsync(v => v.UserId == userId);
        }
    }
}
