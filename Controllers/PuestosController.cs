using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRSRH_EXO.DatabaseContext;
using SRSRH_EXO.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SRSRH_EXO.DTOs;

namespace SRSRH_EXO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PuestosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PuestosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Puestos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PuestoDto>>> GetPuestos()
        {
            return await _context.Puestos
                .Include(p => p.Candidatos)
                .Include(p => p.Empleados)
                .Select(p => MapToDto(p))
                .ToListAsync();
        }

        // GET: api/Puestos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PuestoDto>> GetPuesto(int id)
        {
            var puesto = await _context.Puestos
                .Include(p => p.Candidatos)
                .Include(p => p.Empleados)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (puesto == null)
            {
                return NotFound();
            }

            return MapToDto(puesto);
        }

        // POST: api/Puestos
        [HttpPost]
        public async Task<ActionResult<PuestoDto>> PostPuesto(PuestoCreateDto dto)
        {
            if (await _context.Puestos.AnyAsync(p => p.Nombre == dto.Nombre))
            {
                return BadRequest("Ya existe un puesto con este nombre");
            }

            if (dto.SalarioMinimo >= dto.SalarioMaximo)
            {
                return BadRequest("El salario mínimo debe ser menor al salario máximo");
            }

            var puesto = new Puesto
            {
                Nombre = dto.Nombre,
                NivelRiesgo = dto.NivelRiesgo,
                SalarioMinimo = dto.SalarioMinimo,
                SalarioMaximo = dto.SalarioMaximo,
                Estado = dto.Estado
            };

            _context.Puestos.Add(puesto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPuesto), new { id = puesto.Id }, MapToDto(puesto));
        }

        // PUT: api/Puestos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPuesto(int id, PuestoUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID de puesto no coincide");
            }

            var puesto = await _context.Puestos.FindAsync(id);
            if (puesto == null)
            {
                return NotFound();
            }

            if (await _context.Puestos.AnyAsync(p => p.Nombre == dto.Nombre && p.Id != id))
            {
                return BadRequest("Ya existe otro puesto con este nombre");
            }

            if (dto.SalarioMinimo >= dto.SalarioMaximo)
            {
                return BadRequest("El salario mínimo debe ser menor al salario máximo");
            }

            puesto.Nombre = dto.Nombre;
            puesto.NivelRiesgo = dto.NivelRiesgo;
            puesto.SalarioMinimo = dto.SalarioMinimo;
            puesto.SalarioMaximo = dto.SalarioMaximo;
            puesto.Estado = dto.Estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuestoExists(id))
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

        // DELETE: api/Puestos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePuesto(int id)
        {
            var puesto = await _context.Puestos
                .Include(p => p.Candidatos)
                .Include(p => p.Empleados)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (puesto == null)
            {
                return NotFound();
            }

            if (puesto.Candidatos.Any() || puesto.Empleados.Any())
            {
                return BadRequest("No se puede eliminar: El puesto tiene candidatos o empleados asociados");
            }

            _context.Puestos.Remove(puesto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PuestoExists(int id)
        {
            return _context.Puestos.Any(e => e.Id == id);
        }

        private static PuestoDto MapToDto(Puesto puesto)
        {
            return new PuestoDto
            {
                Id = puesto.Id,
                Nombre = puesto.Nombre,
                NivelRiesgo = puesto.NivelRiesgo,
                SalarioMinimo = puesto.SalarioMinimo,
                SalarioMaximo = puesto.SalarioMaximo,
                Estado = puesto.Estado,
                TotalCandidatos = puesto.Candidatos.Count,
                TotalEmpleados = puesto.Empleados.Count
            };
        }
    }

   
}
