using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRSRH_EXO.DatabaseContext;
using SRSRH_EXO.Models;

namespace SRSRH_EXO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapacitacionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CapacitacionesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Capacitacion>>> GetCapacitaciones()
            => await _context.Capacitaciones.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Capacitacion>> GetCapacitacion(int id)
        {
            var capacitacion = await _context.Capacitaciones.FindAsync(id);
            return capacitacion ?? (ActionResult<Capacitacion>)NotFound();
        }

        // POST: api/Capacitaciones
        [HttpPost]
        public async Task<ActionResult<Capacitacion>> PostCapacitacion(Capacitacion capacitacion)
        {
            // Validar fechas
            if (capacitacion.FechaHasta < capacitacion.FechaDesde)
                return BadRequest("Fecha final debe ser posterior a la inicial");

            _context.Capacitaciones.Add(capacitacion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCapacitacion), new { id = capacitacion.Id }, capacitacion);
        }

        // PUT: api/Capacitaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCapacitacion(int id, Capacitacion capacitacion)
        {
            if (id != capacitacion.Id) return BadRequest();
            if (capacitacion.FechaHasta < capacitacion.FechaDesde)
                return BadRequest("Rango de fechas inválido");

            _context.Entry(capacitacion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Capacitaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCapacitacion(int id)
        {
            var capacitacion = await _context.Capacitaciones.FindAsync(id);
            if (capacitacion == null) return NotFound();

            _context.Capacitaciones.Remove(capacitacion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
