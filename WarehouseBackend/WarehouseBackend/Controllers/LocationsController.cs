using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WarehouseBackend.Models;
using static WarehouseBackend.Models.Location_DTO;


namespace WarehouseBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public LocationsController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: api/Locations
        [HttpGet] //Működik
        public async Task<ActionResult> GetAllLocations()
        {
             
                var locations=await _context.Locations.ToListAsync();

                if (locations!=null)
                {
                    return Ok(new { Result = locations,message="Sikres lekérdezés"});
                }

                Exception e = new();
                return BadRequest(new { Results = "", Message = e.Message });
            
            
        }

        // GET: api/Locations/5
        [HttpGet("{id}")] //Működik
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return location;
        }

        // PUT: api/Locations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")] //Működik
        public async Task<ActionResult> UpdateLocation(int id, UpdateLocationDTO updateLocationDTO)
        {
            var existingLocation=await _context.Locations.FindAsync(id);

            if (existingLocation!=null)
            {
                existingLocation.LocationName = updateLocationDTO.LocationName;
                existingLocation.LocationDescription = updateLocationDTO.LocationDescroption;
                existingLocation.LocationCapacity=updateLocationDTO.LocationCapacity;

                _context.Locations.Update(existingLocation);
                await _context.SaveChangesAsync();

                return Ok();

            }

            return NotFound();
        }

        // POST: api/Locations 
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost] //Működik
        public async Task<ActionResult> AddLocation(CreateLocationDTO createLocationDTO) 
        {

            var location = new Location
            { 
               LocationName=createLocationDTO.LocationName,
               LocationDescription=createLocationDTO.LocationDescription,
               LocationCapacity=createLocationDTO.LocationCapacity,
                                 
            };

            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();

                        

            return CreatedAtAction("GetLocation", new { id = location.LocationId }, location);

           
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")] //Működik
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.LocationId == id);
        }
    }
}
