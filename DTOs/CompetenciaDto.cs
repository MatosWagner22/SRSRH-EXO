using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.DTOs
{
    // DTOs
    public class CompetenciaBaseDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Descripcion { get; set; }

        [Required]
        public bool Estado { get; set; } = true;
    }

    public class CompetenciaCreateDto : CompetenciaBaseDto { }

    public class CompetenciaUpdateDto : CompetenciaBaseDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class CompetenciaDto : CompetenciaUpdateDto
    {
        public int CantidadCandidatos { get; set; }
    }
}
