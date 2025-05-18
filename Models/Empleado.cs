using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.Models
{
    public class Empleado
    {
        public int Id { get; set; }

        public string Cedula { get; set; }

        public string Nombre { get; set; }

        public DateTime FechaIngreso { get; set; }

        public string Departamento { get; set; }

        public int PuestoId { get; set; }
        public Puesto Puesto { get; set; }

        public decimal SalarioMensual { get; set; }

        public string Estado { get; set; }
    }
}
