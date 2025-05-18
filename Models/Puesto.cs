using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.Models
{
    public class Puesto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string NivelRiesgo { get; set; }

        public decimal SalarioMinimo { get; set; }

        public decimal SalarioMaximo { get; set; }

        public bool Estado { get; set; } = true;
    }
}
