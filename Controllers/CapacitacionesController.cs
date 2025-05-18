using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRSRH_EXO.DatabaseContext;
using SRSRH_EXO.DTOs;
using SRSRH_EXO.Models;
using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapacitacionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CapacitacionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Capacitaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CapacitacionDto>>> GetCapacitaciones()
        {
            return await _context.Capacitaciones
                .Include(c => c.Candidatos)
                .Select(c => MapToDto(c))
                .ToListAsync();
        }

        // GET: api/Capacitaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CapacitacionDto>> GetCapacitacion(int id)
        {
            var capacitacion = await _context.Capacitaciones
                .Include(c => c.Candidatos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (capacitacion == null)
            {
                return NotFound();
            }

            return MapToDto(capacitacion);
        }

        // POST: api/Capacitaciones
        [HttpPost]
        public async Task<ActionResult<CapacitacionDto>> PostCapacitacion(CapacitacionCreateDto dto)
        {
            if (await _context.Capacitaciones.AnyAsync(c => c.Descripcion == dto.Descripcion))
            {
                return BadRequest("Ya existe una capacitación con esta descripción");
            }

            var capacitacion = new Capacitacion
            {
                Descripcion = dto.Descripcion,
                Nivel = dto.Nivel,
                FechaDesde = dto.FechaDesde,
                FechaHasta = dto.FechaHasta,
                Institucion = dto.Institucion
            };

            _context.Capacitaciones.Add(capacitacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCapacitacion), new { id = capacitacion.Id }, MapToDto(capacitacion));
        }

        // PUT: api/Capacitaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCapacitacion(int id, CapacitacionUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID de capacitación no coincide");
            }

            var capacitacion = await _context.Capacitaciones.FindAsync(id);
            if (capacitacion == null)
            {
                return NotFound();
            }

            if (await _context.Capacitaciones.AnyAsync(c => c.Descripcion == dto.Descripcion && c.Id != id))
            {
                return BadRequest("Ya existe otra capacitación con esta descripción");
            }

            // Validar fechas
            if (dto.FechaHasta < dto.FechaDesde)
            {
                return BadRequest("La fecha de fin no puede ser anterior a la fecha de inicio");
            }

            capacitacion.Descripcion = dto.Descripcion;
            capacitacion.Nivel = dto.Nivel;
            capacitacion.FechaDesde = dto.FechaDesde;
            capacitacion.FechaHasta = dto.FechaHasta;
            capacitacion.Institucion = dto.Institucion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CapacitacionExists(id))
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

        // DELETE: api/Capacitaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCapacitacion(int id)
        {
            var capacitacion = await _context.Capacitaciones
                .Include(c => c.Candidatos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (capacitacion == null)
            {
                return NotFound();
            }

            if (capacitacion.Candidatos.Any())
            {
                return BadRequest("No se puede eliminar: La capacitación está asignada a candidatos");
            }

            _context.Capacitaciones.Remove(capacitacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CapacitacionExists(int id)
        {
            return _context.Capacitaciones.Any(e => e.Id == id);
        }

        private static CapacitacionDto MapToDto(Capacitacion capacitacion)
        {
            return new CapacitacionDto
            {
                Id = capacitacion.Id,
                Descripcion = capacitacion.Descripcion,
                Nivel = capacitacion.Nivel,
                FechaDesde = capacitacion.FechaDesde,
                FechaHasta = capacitacion.FechaHasta,
                Institucion = capacitacion.Institucion,
                CantidadCandidatos = capacitacion.Candidatos.Count
            };
        }
    }

    
}
