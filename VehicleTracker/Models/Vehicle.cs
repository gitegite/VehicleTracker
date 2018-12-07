using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracker.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Location> Locations { get; set; }
    }
}
