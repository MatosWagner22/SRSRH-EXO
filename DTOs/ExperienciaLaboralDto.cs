using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.DTOs
{
    // DTOs
    public class ExperienciaLaboralBaseDto
    {
        [Required]
        [StringLength(100)]
        public string Empresa { get; set; }

        [Required]
        [StringLength(100)]
        public string PuestoOcupado { get; set; }

        [Required]
        public DateTime FechaDesde { get; set; }

        [Required]
        public DateTime FechaHasta { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salario { get; set; }

        [Required]
        public int CandidatoId { get; set; }
    }

    public class ExperienciaLaboralCreateDto : ExperienciaLaboralBaseDto { }

    public class ExperienciaLaboralUpdateDto : ExperienciaLaboralBaseDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class ExperienciaLaboralDto : ExperienciaLaboralUpdateDto
    {
        public string CandidatoNombre { get; set; }
    }
}
