using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading.Tasks;
using VehicleTracker.Models;

namespace VehicleTracker.Data
{
    public interface IVehicleTrackerContext : IDisposable
    {
        DbSet<Location> Location { get; set; }
        DbSet<Vehicle> Vehicle { get; set; }

        Task<int> SaveChangesAsync();
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}