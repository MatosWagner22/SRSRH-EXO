using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRSRH_EXO.DatabaseContext;
using SRSRH_EXO.Models;

namespace SRSRH_EXO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CandidatosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Candidatos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandidatoDto>>> GetCandidatos()
        {
            return await _context.Candidatos
                .Include(c => c.Puesto)
                .Include(c => c.Competencias)
                .Include(c => c.Idiomas)
                .Include(c => c.Capacitaciones)
                .Include(c => c.Experiencias)
                .Select(c => MapToDto(c))
                .ToListAsync();
        }

        // GET: api/Candidatos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CandidatoDto>> GetCandidato(int id)
        {
            var candidato = await _context.Candidatos
                .Include(c => c.Puesto)
                .Include(c => c.Competencias)
                .Include(c => c.Idiomas)
                .Include(c => c.Capacitaciones)
                .Include(c => c.Experiencias)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidato == null)
            {
                return NotFound();
            }

            return MapToDto(candidato);
        }

        // POST: api/Candidatos
        [HttpPost]
        public async Task<ActionResult<CandidatoDto>> PostCandidato(CandidatoCreateDto dto)
        {
            var candidato = new Candidato
            {
                Cedula = dto.Cedula,
                Nombre = dto.Nombre,
                PuestoId = dto.PuestoId,
                Departamento = dto.Departamento,
                SalarioAspirado = dto.SalarioAspirado,
                RecomendadoPor = dto.RecomendadoPor
            };

            // Manejo de relaciones
            await ManejarRelaciones(candidato, dto);

            _context.Candidatos.Add(candidato);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCandidato), new { id = candidato.Id }, MapToDto(candidato));
        }

        // PUT: api/Candidatos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCandidato(int id, CandidatoUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var candidato = await _context.Candidatos
                .Include(c => c.Competencias)
                .Include(c => c.Idiomas)
                .Include(c => c.Capacitaciones)
                .Include(c => c.Experiencias)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidato == null)
            {
                return NotFound();
            }

            // Actualizar propiedades simples
            candidato.Cedula = dto.Cedula;
            candidato.Nombre = dto.Nombre;
            candidato.PuestoId = dto.PuestoId;
            candidato.Departamento = dto.Departamento;
            candidato.SalarioAspirado = dto.SalarioAspirado;
            candidato.RecomendadoPor = dto.RecomendadoPor;

            // Manejo de relaciones
            await ManejarRelaciones(candidato, dto);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidatoExists(id))
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

        // DELETE: api/Candidatos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidato(int id)
        {
            var candidato = await _context.Candidatos
                .Include(c => c.Competencias)
                .Include(c => c.Idiomas)
                .Include(c => c.Capacitaciones)
                .Include(c => c.Experiencias)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidato == null)
            {
                return NotFound();
            }

            _context.Candidatos.Remove(candidato);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CandidatoExists(int id)
        {
            return _context.Candidatos.Any(e => e.Id == id);
        }

        private async Task ManejarRelaciones(Candidato candidato, CandidatoBaseDto dto)
        {
            // Competencias
            var competencias = await _context.Competencias
                .Where(c => dto.CompetenciasIds.Contains(c.Id))
                .ToListAsync();
            candidato.Competencias = competencias;

            // Idiomas
            var idiomas = await _context.Idiomas
                .Where(i => dto.IdiomasIds.Contains(i.Id))
                .ToListAsync();
            candidato.Idiomas = idiomas;

            // Capacitaciones
            var capacitaciones = await _context.Capacitaciones
                .Where(c => dto.CapacitacionesIds.Contains(c.Id))
                .ToListAsync();
            candidato.Capacitaciones = capacitaciones;

            // Experiencia Laboral
            if (dto.Experiencias != null)
            {
                foreach (var expDto in dto.Experiencias)
                {
                    var experiencia = new ExperienciaLaboral
                    {
                        Empresa = expDto.Empresa,
                        PuestoOcupado = expDto.PuestoOcupado,
                        FechaDesde = expDto.FechaDesde,
                        FechaHasta = expDto.FechaHasta,
                        Salario = expDto.Salario
                    };
                    candidato.Experiencias.Add(experiencia);
                }
            }
        }

        private static CandidatoDto MapToDto(Candidato candidato)
        {
            return new CandidatoDto
            {
                Id = candidato.Id,
                Cedula = candidato.Cedula,
                Nombre = candidato.Nombre,
                PuestoId = candidato.PuestoId,
                PuestoNombre = candidato.Puesto?.Nombre,
                Departamento = candidato.Departamento,
                SalarioAspirado = candidato.SalarioAspirado,
                Competencias = candidato.Competencias.Select(c => c.Descripcion).ToList(),
                Idiomas = candidato.Idiomas.Select(i => i.Nombre).ToList(),
                Capacitaciones = candidato.Capacitaciones.Select(c => c.Descripcion).ToList(),
                Experiencias = candidato.Experiencias.Select(e => new ExperienciaLaboralDto
                {
                    Empresa = e.Empresa,
                    PuestoOcupado = e.PuestoOcupado,
                    FechaDesde = e.FechaDesde,
                    FechaHasta = e.FechaHasta,
                    Salario = e.Salario
                }).ToList(),
                RecomendadoPor = candidato.RecomendadoPor
            };
        }
    }

    // DTOs
    public class CandidatoBaseDto
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public int PuestoId { get; set; }
        public string Departamento { get; set; }
        public decimal SalarioAspirado { get; set; }
        public List<int> CompetenciasIds { get; set; } = new List<int>();
        public List<int> IdiomasIds { get; set; } = new List<int>();
        public List<int> CapacitacionesIds { get; set; } = new List<int>();
        public List<ExperienciaLaboralDto> Experiencias { get; set; } = new List<ExperienciaLaboralDto>();
        public string RecomendadoPor { get; set; }
    }

    public class CandidatoCreateDto : CandidatoBaseDto { }

    public class CandidatoUpdateDto : CandidatoBaseDto
    {
        public int Id { get; set; }
    }

    public class CandidatoDto
    {
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public int PuestoId { get; set; }
        public string PuestoNombre { get; set; }
        public string Departamento { get; set; }
        public decimal SalarioAspirado { get; set; }
        public List<string> Competencias { get; set; }
        public List<string> Idiomas { get; set; }
        public List<string> Capacitaciones { get; set; }
        public List<ExperienciaLaboralDto> Experiencias { get; set; }
        public string RecomendadoPor { get; set; }
    }

    public class ExperienciaLaboralDto
    {
        public string Empresa { get; set; }
        public string PuestoOcupado { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public decimal Salario { get; set; }
    }
}
