using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.DTOs
{
    // DTOs
    public class CapacitacionBaseDto
    {
        public string Descripcion { get; set; }

        public string Nivel { get; set; }

        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        public string Institucion { get; set; }
    }

    public class CapacitacionCreateDto : CapacitacionBaseDto { }

    public class CapacitacionUpdateDto : CapacitacionBaseDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class CapacitacionDto : CapacitacionUpdateDto
    {
        public int CantidadCandidatos { get; set; }
    }
}
