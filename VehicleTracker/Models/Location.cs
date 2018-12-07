using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracker.Models
{
    public class Location
    {
        public Guid Id { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        public DateTime TimeOfRecord { get; set; }

        [Required]
        public Guid VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }
    }
}