using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracker.Models
{
    public class User : IdentityUser
    {
        public Vehicle Vehicle { get; set; }
    }
}
