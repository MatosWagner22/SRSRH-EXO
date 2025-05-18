using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.Models
{
    public class Competencia
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public bool Estado { get; set; } = true;
    }
}
