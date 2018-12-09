using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleTracker.Data;
using VehicleTracker.Models;

namespace VehicleTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IVehicleTrackerContext _context;

        public LocationsController(IVehicleTrackerContext context)
        {
            _context = context;
        }

        [HttpGet("{id}/Date")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetVehicleLocationByDate([FromRoute] Guid id, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var locations = await _context.Location
                                .Where(l => l.TimeOfRecord >= from && l.TimeOfRecord >= to && l.VehicleId == id)
                                .Select(l => new { l.Id, l.Latitude, l.Longitude, l.TimeOfRecord })
                                .ToArrayAsync();

            if (locations == null)
            {
                return NotFound();
            }

            return Ok(locations);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCurrentVehicleLocation([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentLocation = await _context.Location
                                    .Where(l => l.VehicleId == id)
                                    .OrderByDescending(l => l.TimeOfRecord)
                                    .FirstOrDefaultAsync();

            if (currentLocation == null)
            {
                return NotFound();
            }

            return Ok(currentLocation);
        }

        [HttpPost]
        public async Task<IActionResult> PostLocation([FromBody] Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!VehicleExists(location.VehicleId))
            {
                return BadRequest("Vehicle not found");
            }

            var userId = new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Since one vehicle should have only one user, it's fine to use .Single here
            if (location.VehicleId != _context.Vehicle.Single(v => v.UserId == userId).Id)
            {
                return BadRequest("Cannot update other vehicle's location");
            }

            location.TimeOfRecord = DateTime.UtcNow;
            _context.Location.Add(location);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCurrentVehicleLocation", new { id = location.VehicleId }, location);
        }

        private bool VehicleExists(Guid id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }
    }
}