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
    public class LocationsController : ControllerBase
    {
        private readonly IVehicleTrackerContext _context;

        public LocationsController(IVehicleTrackerContext context)
        {
            _context = context;
        }

        // GET: api/Locations
        [HttpGet]
        public IEnumerable<Location> GetAllLocation()
        {
            return _context.Location;
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var location = await _context.Location.FindAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        // POST: api/Locations
        [HttpPost]
        public async Task<IActionResult> PostLocation([FromBody] Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Location.Add(location);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new { id = location.Id }, location);
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var location = await _context.Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.Location.Remove(location);
            await _context.SaveChangesAsync();

            return Ok(location);
        }

        private bool LocationExists(Guid id)
        {
            return _context.Location.Any(e => e.Id == id);
        }
    }
}