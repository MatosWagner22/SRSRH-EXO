using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.DTOs
{
    // DTOs
    public class PuestoBaseDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(10)]
        public string NivelRiesgo { get; set; } // "Alto", "Medio", "Bajo"

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalarioMinimo { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalarioMaximo { get; set; }

        [Required]
        public bool Estado { get; set; } = true;
    }

    public class PuestoCreateDto : PuestoBaseDto { }

    public class PuestoUpdateDto : PuestoBaseDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class PuestoDto : PuestoUpdateDto
    {
        public int TotalCandidatos { get; set; }
        public int TotalEmpleados { get; set; }
    }
}
