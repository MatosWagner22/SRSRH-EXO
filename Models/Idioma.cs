using System.ComponentModel.DataAnnotations;

namespace SRSRH_EXO.Models
{
    public class Idioma
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public bool Estado { get; set; } = true;
    }
}
