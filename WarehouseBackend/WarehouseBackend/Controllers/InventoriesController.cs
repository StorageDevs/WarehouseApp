using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using WarehouseBackend.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;




namespace WarehouseBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public InventoriesController(WarehouseContext context)
        {
            _context = context;
        }

        #region // GET: api/Inventories, all in inventory table
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventories()
        {
            try
            {
                var result = await
                            (
                                from inventory in _context.Inventories
                                join material in _context.Materials
                                on inventory.MaterialId equals material.MaterialId
                                join location in _context.Locations
                                on inventory.LocationId equals location.LocationId
                                select new
                                {
                                    MaterialNumber=material.MaterialNumber,
                                    MaterialDescription = material.MaterialDescription,
                                    LocationName = location.LocationName,
                                    Quantity = inventory.Quantity
                                }
                            )
                            .ToListAsync();
                return Ok(new { Result = result, message = "Successfull request" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "", Message = ex.Message });
            }

        }
        #endregion


        #region // GET: api/Inventories/5
        [HttpGet("CustomQuery")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventories([FromQuery] int? materialId, [FromQuery] int? locationId)
        {
            try
            {
                if (!materialId.HasValue && !locationId.HasValue)
                {
                    return BadRequest(new { Result = "", message = "At least one of materialId or locationId must be provided." });
                }

                var query = from inventory in _context.Inventories
                            join material in _context.Materials
                            on inventory.MaterialId equals material.MaterialId
                            join location in _context.Locations
                            on inventory.LocationId equals location.LocationId
                            select new
                            {
                                MaterialId = material.MaterialId,
                                MaterialNumber = material.MaterialNumber,
                                MaterialDescription = material.MaterialDescription,
                                LocationId = location.LocationId,
                                LocationName = location.LocationName,
                                Quantity = inventory.Quantity
                            };

                if (materialId.HasValue)
                {
                    query = query.Where(i => i.MaterialId == materialId.Value);
                }
                if (locationId.HasValue)
                {
                    query = query.Where(i => i.LocationId == locationId.Value);
                }

                var result = await query.ToListAsync();


                if (result.Any())
                {
                    return Ok(new { Result = result, message = "Successfull request" });
                }

                return NotFound();

                

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "", Message = ex.Message });
            }
        }
        #endregion
   
    }
}
