using SRSRH_EXO.Controllers;

namespace SRSRH_EXO.DTOs
{
    public class CandidatoBaseDto
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public int PuestoId { get; set; }
        public string Departamento { get; set; }
        public decimal SalarioAspirado { get; set; }
        public List<int> CompetenciasIds { get; set; } = new List<int>();
        public List<int> IdiomasIds { get; set; } = new List<int>();
        public List<int> CapacitacionesIds { get; set; } = new List<int>();
        public List<ExperienciaLaboralDto> Experiencias { get; set; } = new List<ExperienciaLaboralDto>();
        public string RecomendadoPor { get; set; }
    }

    public class CandidatoCreateDto : CandidatoBaseDto { }

    public class CandidatoUpdateDto : CandidatoBaseDto
    {
        public int Id { get; set; }
    }

    public class CandidatoDto
    {
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public int PuestoId { get; set; }
        public string PuestoNombre { get; set; }
        public string Departamento { get; set; }
        public decimal SalarioAspirado { get; set; }
        public List<string> Competencias { get; set; }
        public List<string> Idiomas { get; set; }
        public List<string> Capacitaciones { get; set; }
        public List<ExperienciaLaboralDto> Experiencias { get; set; }
        public string RecomendadoPor { get; set; }
    }
}
