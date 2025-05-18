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
    public class CompetenciasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CompetenciasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Competencias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompetenciaDto>>> GetCompetencias()
        {
            return await _context.Competencias
                .Include(c => c.Candidatos)
                .Select(c => MapToDto(c))
                .ToListAsync();
        }

        // GET: api/Competencias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompetenciaDto>> GetCompetencia(int id)
        {
            var competencia = await _context.Competencias
                .Include(c => c.Candidatos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (competencia == null)
            {
                return NotFound();
            }

            return MapToDto(competencia);
        }

        // POST: api/Competencias
        [HttpPost]
        public async Task<ActionResult<CompetenciaDto>> PostCompetencia(CompetenciaCreateDto dto)
        {
            if (await _context.Competencias.AnyAsync(c => c.Descripcion == dto.Descripcion))
            {
                return BadRequest("Ya existe una competencia con esta descripción");
            }

            var competencia = new Competencia
            {
                Descripcion = dto.Descripcion,
                Estado = dto.Estado
            };

            _context.Competencias.Add(competencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompetencia), new { id = competencia.Id }, MapToDto(competencia));
        }

        // PUT: api/Competencias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompetencia(int id, CompetenciaUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID de competencia no coincide");
            }

            var competencia = await _context.Competencias.FindAsync(id);
            if (competencia == null)
            {
                return NotFound();
            }

            if (await _context.Competencias.AnyAsync(c => c.Descripcion == dto.Descripcion && c.Id != id))
            {
                return BadRequest("Ya existe otra competencia con esta descripción");
            }

            competencia.Descripcion = dto.Descripcion;
            competencia.Estado = dto.Estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompetenciaExists(id))
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

        // DELETE: api/Competencias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompetencia(int id)
        {
            var competencia = await _context.Competencias
                .Include(c => c.Candidatos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (competencia == null)
            {
                return NotFound();
            }

            if (competencia.Candidatos.Any())
            {
                return BadRequest("No se puede eliminar: Competencia está asignada a candidatos");
            }

            _context.Competencias.Remove(competencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompetenciaExists(int id)
        {
            return _context.Competencias.Any(e => e.Id == id);
        }

        private static CompetenciaDto MapToDto(Competencia competencia)
        {
            return new CompetenciaDto
            {
                Id = competencia.Id,
                Descripcion = competencia.Descripcion,
                Estado = competencia.Estado,
                CantidadCandidatos = competencia.Candidatos.Count
            };
        }
    }

    
}
