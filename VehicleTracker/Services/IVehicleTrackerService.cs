using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracker.Models;

namespace VehicleTracker.Services
{
    public interface IVehicleTrackerService
    {
        Task<Vehicle> GetVehicleById(Guid id);
        Task RegisterVehicle(Vehicle vehicle);
        Task UpdateVehicle(Vehicle vehicle);
        Task RemoveVehicle(Vehicle vehicle);
        Task<IReadOnlyCollection<Location>> GetVehicleLocationByDate(Guid id, DateTime from, DateTime to);
        Task<Location> GetCurrentVehicleLocation(Guid id);
        Task RecordCurrentLocation(Guid vehicleId, Location location);
        Task<Vehicle> GetVehicleByUser(string userId);
    }
}