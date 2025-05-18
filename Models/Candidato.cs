using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.Models
{
    public class Candidato
    {
        public int Id { get; set; }

        public string Cedula { get; set; }

        public string Nombre { get; set; }

        public int PuestoId { get; set; }
        public Puesto Puesto { get; set; }

        public string Departamento { get; set; }

        public decimal SalarioAspirado { get; set; }

        public List<Competencia> Competencias { get; set; } = new List<Competencia>();
        public List<Idioma> Idiomas { get; set; } = new List<Idioma>();
        public List<Capacitacion> Capacitaciones { get; set; } = new List<Capacitacion>();

        // Relación one-to-many
        public List<ExperienciaLaboral> Experiencias { get; set; } = new List<ExperienciaLaboral>();
        public string RecomendadoPor { get; set; }
    }
}
