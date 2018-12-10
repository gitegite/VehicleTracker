using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VehicleTracker.Models;

namespace VehicleTracker.Data
{
    public class VehicleTrackerContext : IdentityDbContext<User>, IVehicleTrackerContext
    {
        public VehicleTrackerContext(DbContextOptions<VehicleTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<Location> Location { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            User user = new User
            {
                UserName = "John Doe",
                Id = "1203521D-4FE4-4906-819B-C19016E42A03"
            };

            Guid vehicleId = new Guid("62d65fe9-35d2-4529-983e-d8f92441dfd8");
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle
                {
                    Id = vehicleId,
                    Name = "Mercedes",
                    UserId = user.Id
                }
            );
            modelBuilder.Entity<Location>().HasData(
                new Location
                {
                    VehicleId = vehicleId,
                    Id = new Guid("0E5CC4B8-4C97-4F8A-B0EF-46D76AEB1E98"),
                    Latitude = 100,
                    Longitude = 200
                }
            );

            modelBuilder.Entity<User>()
            .HasOne(u => u.Vehicle)
            .WithOne(v => v.User)
            .HasForeignKey<Vehicle>(v => v.UserId);
        }
    }
}
