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
using VehicleTracker.Services;

namespace VehicleTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IVehicleTrackerService _vehicleTrackerService;

        public LocationsController(IVehicleTrackerService vehicleTrackerService)
        {
            _vehicleTrackerService = vehicleTrackerService;
        }

        [HttpGet("{id}/Date")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetVehicleLocationByDate([FromRoute] Guid id, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var locations = await _vehicleTrackerService.GetVehicleLocationByDate(id, from, to);

            if (locations == null || !locations.Any())
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

            var currentLocation = await _vehicleTrackerService.GetCurrentVehicleLocation(id);

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

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Vehicle vehicle = await _vehicleTrackerService.GetVehicleByUser(userId);
            if (vehicle == null)
            {
                return BadRequest("Vehicle not found");
            }

            await _vehicleTrackerService.RecordCurrentLocation(vehicle.Id, location);

            return CreatedAtAction("GetCurrentVehicleLocation", new { id = location.VehicleId }, location);
        }
    }
}