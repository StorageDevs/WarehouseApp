using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using WarehouseBackend.Models;
using static WarehouseBackend.Models.Material_DTO;

namespace WarehouseBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public MaterialsController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: api/Materials
        [HttpGet] //Working
        public async Task<ActionResult<IEnumerable<Material>>> GetAllMaterials()
        {

            var material = await _context.Materials.ToListAsync();

            

            if (material!=null )
            {
                return Ok(new {Result = material, message = "Successfull request" });

            }
            Exception e = new Exception();
            return BadRequest(new { Results = "something went wrong", Message = e.Message });

            
        }



        // GET: api/Materials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> GetMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound();
            }

            return material;
        }

        // PUT: api/Materials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(int id, UpdateMaterial updateMaterial)
        {

            var existingMaterial = await _context.Materials.FindAsync(id);

            if (existingMaterial != null)
            {
                existingMaterial.MaterialNumber = updateMaterial.MaterialNumber;
                existingMaterial.MaterialDescription = updateMaterial.MaterialDescription;
                existingMaterial.Unit = updateMaterial.Unit;
                existingMaterial.PriceUnit = updateMaterial.PriceUnit;


                _context.Materials.Update(existingMaterial);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound();



        }

        // POST: api/Materials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost] //működik
        public async Task<ActionResult<Material>> AddMaterial(CreateMaterial creatematerial)
        {
            var newMaterial = new Material
            {
               MaterialNumber = creatematerial.MaterialNumber,
               MaterialDescription= creatematerial.MaterialDescription,
               Unit=creatematerial.Unit,
               PriceUnit=creatematerial.PriceUnit,

            };

            await _context.Materials.AddAsync(newMaterial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaterial", new { id = newMaterial.MaterialId }, newMaterial);
        }

        // DELETE: api/Materials/5
        [HttpDelete("{id}")] //működik
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.MaterialId == id);
        }
    }
}
