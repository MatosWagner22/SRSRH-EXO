using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SRSRH_EXO.DatabaseContext;
using SRSRH_EXO.Models;

namespace SRSRH_EXO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdiomasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IdiomasController(AppDbContext context) => _context = context;

        // GET: api/Idiomas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Idioma>>> GetIdiomas()
            => await _context.Idiomas.ToListAsync();

        // GET: api/Idiomas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Idioma>> GetIdioma(int id)
        {
            var idioma = await _context.Idiomas.FindAsync(id);
            return idioma ?? (ActionResult<Idioma>)NotFound();
        }

        // POST: api/Idiomas
        [HttpPost]
        public async Task<ActionResult<Idioma>> PostIdioma(Idioma idioma)
        {
            if (string.IsNullOrEmpty(idioma.Nombre))
                return BadRequest("El nombre del idioma es requerido");

            _context.Idiomas.Add(idioma);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetIdioma), new { id = idioma.Id }, idioma);
        }

        // PUT: api/Idiomas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdioma(int id, Idioma idioma)
        {
            if (id != idioma.Id) return BadRequest();
            if (string.IsNullOrEmpty(idioma.Nombre))
                return BadRequest("Nombre inválido");

            _context.Entry(idioma).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Idiomas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdioma(int id)
        {
            var idioma = await _context.Idiomas.FindAsync(id);
            if (idioma == null) return NotFound();

            // Validar si está en uso
            var enUso = await _context.Candidatos.AnyAsync(c => c.Idiomas.Any(i => i.Id == id));
            if (enUso) return BadRequest("No se puede eliminar: idioma asociado a candidatos");

            _context.Idiomas.Remove(idioma);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
