using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RecursivaChallenge.Models.Entity
{
    public class Socio
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(150)]
        public string Nombre { get; set; }
        public int Edad { get; set; }
        [MaxLength(150)]
        public string Equipo { get; set; }
        [MaxLength(150)]
        public string EstadoCivil { get; set; }
        [MaxLength(150)]
        public string NivelDeEstudios { get; set; }
    }
}
