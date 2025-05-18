using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.Models
{
    public class Capacitacion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Nivel { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Institucion { get; set; }

        // Propiedad de navegación inversa
        public List<Candidato> Candidatos { get; set; } = new List<Candidato>();
    }
}
