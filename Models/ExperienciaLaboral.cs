using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.Models
{
    public class ExperienciaLaboral
    {
        public int Id { get; set; }

        public string Empresa { get; set; }

        public string PuestoOcupado { get; set; }

        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        public decimal Salario { get; set; }

        public int CandidatoId { get; set; }
        public Candidato Candidato { get; set; }
    }
}
