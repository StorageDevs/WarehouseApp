using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        #region// GET: api/Materials
        [Authorize(Roles = "superuser,admin")]
        [HttpGet] 
        public async Task<ActionResult<IEnumerable<Material>>> GetAllMaterials()
        {
            try
            {
                var material = await _context.Materials.ToListAsync();
                if (material!=null )
                {
                    return Ok(new {Result = material, message = "Successfull request" });
                }
                return NotFound(new { material = "", Message = "No such material" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "", Message = ex.Message });
            }
        }
        #endregion

        #region// GET: api/Materials/5
        [Authorize(Roles = "superuser,admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> GetMaterial(int id)
        {
            try
            {
                var material = await _context.Materials.FindAsync(id);
                if (material == null)
                {
                    return NotFound();
                }
                return material;
            }
            catch (Exception ex )
            {
                return StatusCode(500, new { Result = "", Message = ex.Message });
            }
        }
        #endregion

        #region// PUT: api/Materials/5
        [Authorize(Roles = "superuser,admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(int id, UpdateMaterial updateMaterial)
        {

            try
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

                    return Ok(new { Result = existingMaterial, message = "Successfull request" });
                }
                return NotFound(new { Result = "", Message = "No such material" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "", Message = ex.Message });

            }
        }
        #endregion

        #region// POST: api/Materials
        [Authorize(Roles = "superuser,admin")]
        [HttpPost] 
        public async Task<ActionResult<Material>> AddMaterial(CreateMaterial creatematerial)
        {
            try
            {
                var newMaterial = new Material
                {
                    MaterialNumber = creatematerial.MaterialNumber,
                    MaterialDescription = creatematerial.MaterialDescription,
                    Unit = creatematerial.Unit,
                    PriceUnit = creatematerial.PriceUnit,

                };

                await _context.Materials.AddAsync(newMaterial);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetMaterial", new { id = newMaterial.MaterialId }, newMaterial);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Result = "", Message = ex.Message });
            }
        }
        #endregion

        #region// DELETE: api/Materials/5
        [Authorize(Roles = "superuser,admin")]
        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            

            try
            {
                var material = await _context.Materials.FindAsync(id);
                if (material == null)
                {
                    return NotFound();
                }

                _context.Materials.Remove(material);
                await _context.SaveChangesAsync();

                return Ok(new { Result = material, Message = "Deleted succefully" });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Result = "", Message = ex.Message });
            }
        }
        #endregion
       
    }
}
