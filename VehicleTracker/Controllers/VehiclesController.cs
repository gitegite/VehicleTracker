using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleTracker.Models;

namespace VehicleTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleTrackerContext _context;

        public VehiclesController(IVehicleTrackerContext context)
        {
            _context = context;
        }

        // GET: api/Vehicles
        [HttpGet]
        public IEnumerable<Vehicle> GetAllVehicle()
        {
            return _context.Vehicle.Include(v => v.Locations).ToList();
        }

        // GET: api/Vehicles/5
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

        // PUT: api/Vehicles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle([FromRoute] Guid id, [FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vehicle.Id)
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

        // POST: api/Vehicles
        [HttpPost]
        public async Task<IActionResult> PostVehicle([FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Vehicle.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicle", new { id = vehicle.Id }, vehicle);
        }

        // DELETE: api/Vehicles/5
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