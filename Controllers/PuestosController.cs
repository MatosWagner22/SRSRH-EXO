using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRSRH_EXO.DatabaseContext;
using SRSRH_EXO.Models;

namespace SRSRH_EXO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PuestosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PuestosController(AppDbContext context) => _context = context;

        // GET: api/Puestos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Puesto>>> GetPuestos()
            => await _context.Puestos.ToListAsync();

        // GET: api/Puestos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Puesto>> GetPuesto(int id)
        {
            var puesto = await _context.Puestos.FindAsync(id);
            return puesto ?? (ActionResult<Puesto>)NotFound();
        }

        // POST: api/Puestos
        [HttpPost]
        public async Task<ActionResult<Puesto>> PostPuesto(Puesto puesto)
        {
            if (puesto.SalarioMinimo >= puesto.SalarioMaximo)
                return BadRequest("Salario mínimo debe ser menor al máximo");

            _context.Puestos.Add(puesto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPuesto), new { id = puesto.Id }, puesto);
        }

        // PUT: api/Puestos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPuesto(int id, Puesto puesto)
        {
            if (id != puesto.Id) return BadRequest();
            if (puesto.SalarioMinimo >= puesto.SalarioMaximo)
                return BadRequest("Rango salarial inválido");

            _context.Entry(puesto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Puestos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePuesto(int id)
        {
            var puesto = await _context.Puestos.FindAsync(id);
            if (puesto == null) return NotFound();

            // Validar si tiene candidatos asociados
            if (await _context.Candidatos.AnyAsync(c => c.PuestoId == id))
                return BadRequest("No se puede eliminar: puesto asignado a candidatos");

            _context.Puestos.Remove(puesto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
