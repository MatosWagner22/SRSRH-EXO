using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.DTOs
{
    // DTOs
    public class IdiomaBaseDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Nombre { get; set; }

        [Required]
        public bool Estado { get; set; } = true;
    }

    public class IdiomaCreateDto : IdiomaBaseDto { }

    public class IdiomaUpdateDto : IdiomaBaseDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class IdiomaDto : IdiomaUpdateDto
    {
        public int CantidadCandidatos { get; set; }
    }
}
