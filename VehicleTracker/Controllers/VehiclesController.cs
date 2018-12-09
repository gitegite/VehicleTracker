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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleTrackerContext _context;

        public VehiclesController(IVehicleTrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Vehicle> GetAllVehicle()
        {
            return _context.Vehicle.Include(v => v.Locations).ToList();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await _context.Vehicle.FindAsync(id);

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

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostVehicle([FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            vehicle.UserId = new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            _context.Vehicle.Add(vehicle);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicle", new { id = vehicle.Id }, vehicle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();

            return Ok(vehicle);
        }

        private bool VehicleExists(Guid id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }
    }
}