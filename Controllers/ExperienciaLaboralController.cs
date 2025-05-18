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
    public class ExperienciaLaboralController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExperienciaLaboralController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ExperienciaLaboral/candidato/5
        [HttpGet("candidato/{candidatoId}")]
        public async Task<ActionResult<IEnumerable<ExperienciaLaboralDto>>> GetExperienciasPorCandidato(int candidatoId)
        {
            return await _context.ExperienciasLaborales
                .Where(e => e.CandidatoId == candidatoId)
                .Select(e => MapToDto(e))
                .ToListAsync();
        }

        // GET: api/ExperienciaLaboral/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExperienciaLaboralDto>> GetExperienciaLaboral(int id)
        {
            var experiencia = await _context.ExperienciasLaborales
                .Include(e => e.Candidato)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (experiencia == null)
            {
                return NotFound();
            }

            return MapToDto(experiencia);
        }

        // POST: api/ExperienciaLaboral
        [HttpPost]
        public async Task<ActionResult<ExperienciaLaboralDto>> PostExperienciaLaboral(ExperienciaLaboralCreateDto dto)
        {
            // Validar candidato
            if (!await _context.Candidatos.AnyAsync(c => c.Id == dto.CandidatoId))
            {
                return BadRequest("El candidato especificado no existe");
            }

            // Validar fechas
            if (dto.FechaHasta < dto.FechaDesde)
            {
                return BadRequest("La fecha de fin no puede ser anterior a la fecha de inicio");
            }

            var experiencia = new ExperienciaLaboral
            {
                Empresa = dto.Empresa,
                PuestoOcupado = dto.PuestoOcupado,
                FechaDesde = dto.FechaDesde,
                FechaHasta = dto.FechaHasta,
                Salario = dto.Salario,
                CandidatoId = dto.CandidatoId
            };

            _context.ExperienciasLaborales.Add(experiencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExperienciaLaboral), new { id = experiencia.Id }, MapToDto(experiencia));
        }

        // PUT: api/ExperienciaLaboral/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExperienciaLaboral(int id, ExperienciaLaboralUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID de experiencia no coincide");
            }

            var experiencia = await _context.ExperienciasLaborales.FindAsync(id);
            if (experiencia == null)
            {
                return NotFound();
            }

            // Validar fechas
            if (dto.FechaHasta < dto.FechaDesde)
            {
                return BadRequest("La fecha de fin no puede ser anterior a la fecha de inicio");
            }

            experiencia.Empresa = dto.Empresa;
            experiencia.PuestoOcupado = dto.PuestoOcupado;
            experiencia.FechaDesde = dto.FechaDesde;
            experiencia.FechaHasta = dto.FechaHasta;
            experiencia.Salario = dto.Salario;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExperienciaLaboralExists(id))
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

        // DELETE: api/ExperienciaLaboral/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExperienciaLaboral(int id)
        {
            var experiencia = await _context.ExperienciasLaborales.FindAsync(id);
            if (experiencia == null)
            {
                return NotFound();
            }

            _context.ExperienciasLaborales.Remove(experiencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExperienciaLaboralExists(int id)
        {
            return _context.ExperienciasLaborales.Any(e => e.Id == id);
        }

        private static ExperienciaLaboralDto MapToDto(ExperienciaLaboral experiencia)
        {
            return new ExperienciaLaboralDto
            {
                Id = experiencia.Id,
                Empresa = experiencia.Empresa,
                PuestoOcupado = experiencia.PuestoOcupado,
                FechaDesde = experiencia.FechaDesde,
                FechaHasta = experiencia.FechaHasta,
                Salario = experiencia.Salario,
                CandidatoId = experiencia.CandidatoId,
                CandidatoNombre = experiencia.Candidato?.Nombre
            };
        }
    }

    
}
