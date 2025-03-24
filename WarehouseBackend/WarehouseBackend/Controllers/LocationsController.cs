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
using MySqlX.XDevAPI.Common;
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

        #region // GET: api/Locations
        [HttpGet] 
        public async Task<ActionResult> GetAllLocations()
        {
            try
            {
                var locations = await _context.Locations.ToListAsync();

                if (locations.Any())
                {
                    return Ok(new { Result = locations, message = "Successfull request" });
                }
                return NotFound(new { Result = "", Message ="No such location"  });

            }
            catch (Exception ex)
            {
                return StatusCode(500,new { Result = "", Message = ex.Message });
            }
        }
        #endregion

        #region// GET: api/Locations/5
        [HttpGet("{id}")] 
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            try
            {
                var location = await _context.Locations.FindAsync(id);

                if(location == null) 
                { 

                    return NotFound(new { Result = "", Message = "No such location" });
                }
                return Ok(new { Result = location, message = "Successfull request" });
            }
            catch (Exception ex)
            {
               return StatusCode(500,new {Result="",Message=ex.Message}); 
            }
        }
        #endregion

        #region // PUT: api/Locations/5
        [HttpPut("{id}")] 
        public async Task<ActionResult> UpdateLocation(int id, UpdateLocationDTO updateLocationDTO)
        {
            try
            {
                var existingLocation = await _context.Locations.FindAsync(id);

                if (existingLocation!=null)
                {
                    existingLocation.LocationName = updateLocationDTO.LocationName;
                    existingLocation.LocationDescription = updateLocationDTO.LocationDescription;
                    existingLocation.LocationCapacity = updateLocationDTO.LocationCapacity;
                    _context.Locations.Update(existingLocation);

                    await _context.SaveChangesAsync();

                    return Ok(new { Result = existingLocation, message = "Successfull request" });
                }
                return NotFound(new { Result = "", Message = "No such location" });

            }
            catch (Exception ex)
            {
              return StatusCode(500, new { Result = "", Message = ex.Message });
            }

        }
        #endregion

        #region // POST: api/Locations 
       
        [HttpPost] 
        public async Task<ActionResult> AddLocation(CreateLocationDTO createLocationDTO) 
        {
            try
            {
                var location = new Location
                {
                    LocationName = createLocationDTO.LocationName,
                    LocationDescription = createLocationDTO.LocationDescription,
                    LocationCapacity = createLocationDTO.LocationCapacity,

                };

                await _context.Locations.AddAsync(location);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetLocation", new { id = location.LocationId}, location);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "", Message = ex.Message });
            }

           
        }
        #endregion

        #region // DELETE: api/Locations/5
        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                var location = await _context.Locations.FindAsync(id);
                if (location == null)
                {
                    return NotFound(new { Result = "", Message = "No such location" });
                }
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "", Message = ex.Message });
            }

        }

   



        //private bool LocationExists(int id)
        //{
        //    return _context.Locations.Any(e => e.LocationId == id);
        //}




        #endregion
    }
}
