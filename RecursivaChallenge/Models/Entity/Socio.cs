using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RecursivaChallenge.Models.Entity
{
    public class Socio
    {
        [Key]
        public int Id { get; set; }
        [Index(0)]
        [MaxLength(150)]
        public string Nombre { get; set; }
        [Index(1)]
        public int Edad { get; set; }
        [Index(2)]
        [MaxLength(150)]
        public string Equipo { get; set; }
        [Index(3)]
        [MaxLength(150)]
        public string EstadoCivil { get; set; }
        [Index(4)]
        [MaxLength(150)]
        public string NivelDeEstudios { get; set; }
    }
}
