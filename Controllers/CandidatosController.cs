using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRSRH_EXO.DatabaseContext;
using SRSRH_EXO.DTOs;
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

        [HttpPost("{id}/ConvertirAEmpleado")]
        public async Task<ActionResult<EmpleadoDto>> ConvertirCandidatoAEmpleado(int id)
        {
            // Obtener el candidato con sus relaciones
            var candidato = await _context.Candidatos
                .Include(c => c.Puesto)
                .Include(c => c.Competencias)
                .Include(c => c.Idiomas)
                .Include(c => c.Capacitaciones)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidato == null)
            {
                return NotFound();
            }

            // Validar que el candidato tenga un puesto asignado
            if (candidato.Puesto == null)
            {
                return BadRequest("El candidato no tiene un puesto asignado");
            }

            // Crear el empleado a partir del candidato
            var empleado = new Empleado
            {
                Cedula = candidato.Cedula,
                Nombre = candidato.Nombre,
                PuestoId = candidato.PuestoId,
                Departamento = candidato.Departamento,
                SalarioMensual = candidato.SalarioAspirado,
                Estado = "Activo",
                FechaIngreso = DateTime.Now
            };

            // Validar salario con el puesto
            if (empleado.SalarioMensual < candidato.Puesto.SalarioMinimo ||
                empleado.SalarioMensual > candidato.Puesto.SalarioMaximo)
            {
                return BadRequest($"El salario aspirado ({empleado.SalarioMensual}) no está dentro del rango del puesto ({candidato.Puesto.SalarioMinimo} - {candidato.Puesto.SalarioMaximo})");
            }

            // Agregar el empleado
            _context.Empleados.Add(empleado);

            // Eliminar el candidato
            _context.Candidatos.Remove(candidato);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error al convertir el candidato en empleado: " + ex.InnerException?.Message ?? ex.Message);
            }

            // Cargar datos relacionados para el DTO
            await _context.Entry(empleado).Reference(e => e.Puesto).LoadAsync();

            return CreatedAtAction(nameof(EmpleadosController.GetEmpleado),
                "Empleados",
                new { id = empleado.Id },
                MapToEmpleadoDto(empleado));
        }

        // GET: api/Candidatos/filtro
        [HttpGet("filtro")]
        public async Task<ActionResult<IEnumerable<CandidatoDto>>> GetCandidatosFiltrados(
            [FromQuery] int? puestoId,
            [FromQuery] int? competenciaId,
            [FromQuery] int? idiomaId,
            [FromQuery] int? capacitacionId)
        {
            var query = _context.Candidatos
                .Include(c => c.Puesto)
                .Include(c => c.Competencias)
                .Include(c => c.Idiomas)
                .Include(c => c.Capacitaciones)
                .AsQueryable();

            // Aplicar filtros
            if (puestoId.HasValue)
            {
                query = query.Where(c => c.PuestoId == puestoId.Value);
            }

            if (competenciaId.HasValue)
            {
                query = query.Where(c => c.Competencias.Any(comp => comp.Id == competenciaId.Value));
            }

            if (idiomaId.HasValue)
            {
                query = query.Where(c => c.Idiomas.Any(i => i.Id == idiomaId.Value));
            }

            if (capacitacionId.HasValue)
            {
                query = query.Where(c => c.Capacitaciones.Any(cap => cap.Id == capacitacionId.Value));
            }

            var candidatos = await query.ToListAsync();
            return candidatos.Select(c => MapToDto(c)).ToList();
        }

        private static EmpleadoDto MapToEmpleadoDto(Empleado empleado)
        {
            return new EmpleadoDto
            {
                Id = empleado.Id,
                Cedula = empleado.Cedula,
                Nombre = empleado.Nombre,
                FechaIngreso = empleado.FechaIngreso,
                Departamento = empleado.Departamento,
                PuestoId = empleado.PuestoId,
                PuestoNombre = empleado.Puesto?.Nombre,
                SalarioMensual = empleado.SalarioMensual,
                Estado = empleado.Estado
            };
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

    
}
