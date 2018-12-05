using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracker.Models
{
    public class Location
    {
        public Guid Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime TimeOfRecord { get; set; }

        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}