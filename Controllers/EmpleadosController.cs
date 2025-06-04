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
    public class EmpleadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpleadosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Empleados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> GetEmpleados()
        {
            return await _context.Empleados
                .Include(e => e.Puesto)
                .Select(e => MapToDto(e))
                .ToListAsync();
        }

        // GET: api/Empleados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpleadoDto>> GetEmpleado(int id)
        {
            var empleado = await _context.Empleados
                .Include(e => e.Puesto)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (empleado == null)
            {
                return NotFound();
            }

            return MapToDto(empleado);
        }

        [HttpPost]
        public async Task<ActionResult<EmpleadoDto>> PostEmpleado(EmpleadoCreateDto dto)
        {
            // Validar puesto
            var puesto = await _context.Puestos.FindAsync(dto.PuestoId);
            if (puesto == null)
            {
                return BadRequest("El puesto especificado no existe");
            }

            // Validar salario
            if (dto.SalarioMensual < puesto.SalarioMinimo || dto.SalarioMensual > puesto.SalarioMaximo)
            {
                return BadRequest($"El salario debe estar entre {puesto.SalarioMinimo} y {puesto.SalarioMaximo}");
            }

            // Crear nuevo empleado
            var empleado = new Empleado
            {
                Cedula = dto.Cedula,
                Nombre = dto.Nombre,
                PuestoId = dto.PuestoId,
                Departamento = dto.Departamento,
                SalarioMensual = dto.SalarioMensual,
                Estado = dto.Estado,
                FechaIngreso = DateTime.Now
            };

            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            // Cargar datos relacionados para el DTO
            await _context.Entry(empleado).Reference(e => e.Puesto).LoadAsync();

            return CreatedAtAction("GetEmpleado", new { id = empleado.Id }, MapToDto(empleado));
        }


        // PUT: api/Empleados/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpleado(int id, EmpleadoUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID del empleado no coincide");
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            // Validar puesto
            if (!await _context.Puestos.AnyAsync(p => p.Id == dto.PuestoId))
            {
                return BadRequest("El puesto especificado no existe");
            }

            // Validar salario
            var puesto = await _context.Puestos.FindAsync(dto.PuestoId);
            if (dto.SalarioMensual < puesto.SalarioMinimo || dto.SalarioMensual > puesto.SalarioMaximo)
            {
                return BadRequest($"El salario debe estar entre {puesto.SalarioMinimo} y {puesto.SalarioMaximo}");
            }

            empleado.Nombre = dto.Nombre;
            empleado.PuestoId = dto.PuestoId;
            empleado.Departamento = dto.Departamento;
            empleado.SalarioMensual = dto.SalarioMensual;
            empleado.Estado = dto.Estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpleadoExists(id))
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

        // DELETE: api/Empleados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            // Marcar como inactivo en lugar de eliminar
            empleado.Estado = "Inactivo";
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }

        private static EmpleadoDto MapToDto(Empleado empleado)
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
    }

    
}
