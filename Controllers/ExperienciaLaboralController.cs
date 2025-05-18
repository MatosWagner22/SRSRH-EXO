using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRSRH_EXO.DatabaseContext;
using SRSRH_EXO.Models;

namespace SRSRH_EXO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienciaLaboralController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExperienciaLaboralController(AppDbContext context) => _context = context;

        // POST: api/ExperienciaLaboral
        [HttpPost]
        public async Task<ActionResult<ExperienciaLaboral>> PostExperiencia(ExperienciaLaboral experiencia)
        {
            // Validar candidato
            if (!await _context.Candidatos.AnyAsync(c => c.Id == experiencia.CandidatoId))
                return BadRequest("Candidato no existe");

            // Validar fechas
            if (experiencia.FechaHasta < experiencia.FechaDesde)
                return BadRequest("Rango de fechas inválido");

            _context.ExperienciasLaborales.Add(experiencia);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetExperiencia), new { id = experiencia.Id }, experiencia);
        }

        // GET: api/ExperienciaLaboral/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExperienciaLaboral>> GetExperiencia(int id)
        {
            var experiencia = await _context.ExperienciasLaborales
                .Include(e => e.Candidato)
                .FirstOrDefaultAsync(e => e.Id == id);

            return experiencia ?? (ActionResult<ExperienciaLaboral>)NotFound();
        }

        // DELETE: api/ExperienciaLaboral/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExperiencia(int id)
        {
            var experiencia = await _context.ExperienciasLaborales.FindAsync(id);
            if (experiencia == null) return NotFound();

            _context.ExperienciasLaborales.Remove(experiencia);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
