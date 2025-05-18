using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.DTOs
{
    // DTOs
    public class EmpleadoBaseDto
    {
        [Required]
        [StringLength(20)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public int PuestoId { get; set; }

        [Required]
        [StringLength(50)]
        public string Departamento { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalarioMensual { get; set; }

        [Required]
        [StringLength(10)]
        public string Estado { get; set; } // "Activo" o "Inactivo"
    }

    public class EmpleadoCreateDto : EmpleadoBaseDto { }

    public class EmpleadoUpdateDto : EmpleadoBaseDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class EmpleadoDto : EmpleadoUpdateDto
    {
        public DateTime FechaIngreso { get; set; }
        public string PuestoNombre { get; set; }
    }
}
