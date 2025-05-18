using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SRSRH_EXO.DatabaseContext;
using SRSRH_EXO.DTOs;
using SRSRH_EXO.Models;

namespace SRSRH_EXO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdiomasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IdiomasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Idiomas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdiomaDto>>> GetIdiomas()
        {
            return await _context.Idiomas
                .Include(i => i.Candidatos)
                .Select(i => MapToDto(i))
                .ToListAsync();
        }

        // GET: api/Idiomas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IdiomaDto>> GetIdioma(int id)
        {
            var idioma = await _context.Idiomas
                .Include(i => i.Candidatos)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (idioma == null)
            {
                return NotFound();
            }

            return MapToDto(idioma);
        }

        // POST: api/Idiomas
        [HttpPost]
        public async Task<ActionResult<IdiomaDto>> PostIdioma(IdiomaCreateDto dto)
        {
            if (await _context.Idiomas.AnyAsync(i => i.Nombre == dto.Nombre))
            {
                return BadRequest("Ya existe un idioma con este nombre");
            }

            var idioma = new Idioma
            {
                Nombre = dto.Nombre,
                Estado = dto.Estado
            };

            _context.Idiomas.Add(idioma);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIdioma), new { id = idioma.Id }, MapToDto(idioma));
        }

        // PUT: api/Idiomas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdioma(int id, IdiomaUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID de idioma no coincide");
            }

            var idioma = await _context.Idiomas.FindAsync(id);
            if (idioma == null)
            {
                return NotFound();
            }

            if (await _context.Idiomas.AnyAsync(i => i.Nombre == dto.Nombre && i.Id != id))
            {
                return BadRequest("Ya existe otro idioma con este nombre");
            }

            idioma.Nombre = dto.Nombre;
            idioma.Estado = dto.Estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdiomaExists(id))
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

        // DELETE: api/Idiomas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdioma(int id)
        {
            var idioma = await _context.Idiomas
                .Include(i => i.Candidatos)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (idioma == null)
            {
                return NotFound();
            }

            if (idioma.Candidatos.Any())
            {
                return BadRequest("No se puede eliminar: El idioma está asignado a candidatos");
            }

            _context.Idiomas.Remove(idioma);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IdiomaExists(int id)
        {
            return _context.Idiomas.Any(e => e.Id == id);
        }

        private static IdiomaDto MapToDto(Idioma idioma)
        {
            return new IdiomaDto
            {
                Id = idioma.Id,
                Nombre = idioma.Nombre,
                Estado = idioma.Estado,
                CantidadCandidatos = idioma.Candidatos.Count
            };
        }
    }

    
}
