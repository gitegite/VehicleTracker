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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleTrackerService _vehicleTrackerService;

        public VehiclesController(IVehicleTrackerService vehicleTrackerService)
        {
            _vehicleTrackerService = vehicleTrackerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await _vehicleTrackerService.GetVehicleById(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle([FromRoute] Guid id, [FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // vehicle's locations shouldn't be mutated once recorded
            if (id != vehicle.Id || vehicle.Locations.Any())
            {
                return BadRequest();
            }

            await _vehicleTrackerService.UpdateVehicle(vehicle);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostVehicle([FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            vehicle.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (await _vehicleTrackerService.GetVehicleByUser(vehicle.UserId) != null)
            {
                return BadRequest("Only one device per one vehicle allowed");
            }

            await _vehicleTrackerService.RegisterVehicle(vehicle);

            return CreatedAtAction("GetVehicle", new { id = vehicle.Id }, vehicle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await _vehicleTrackerService.GetVehicleById(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            await _vehicleTrackerService.RemoveVehicle(vehicle);

            return Ok(vehicle);
        }
    }
}