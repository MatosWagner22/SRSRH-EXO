using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRSRH_EXO.DatabaseContext;
using SRSRH_EXO.Models;

namespace SRSRH_EXO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpleadosController(AppDbContext context) => _context = context;

        // GET: api/Empleados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
            => await _context.Empleados
                .Include(e => e.Puesto)
                .ToListAsync();

        // PUT: api/Empleados/5 (Actualizar estado)
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> UpdateEstado(int id, [FromBody] string estado)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();

            if (estado != "Activo" && estado != "Inactivo")
                return BadRequest("Estado debe ser 'Activo' o 'Inactivo'");

            empleado.Estado = estado;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Empleados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id)
        {
            var empleado = await _context.Empleados
                .Include(e => e.Puesto)
                .FirstOrDefaultAsync(e => e.Id == id);

            return empleado ?? (ActionResult<Empleado>)NotFound();
        }

        // DELETE: api/Empleados/5 (Baja lógica)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();

            empleado.Estado = "Inactivo";
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
