using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracker.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Location> Locations { get; set; }
    }
}
