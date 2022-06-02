using CsvHelper.Configuration;
using RecursivaChallenge.Models.Entity;
using System.Globalization;

namespace RecursivaChallenge.CsvMap
{
    public class SocioMap : ClassMap<Socio>
    {
        public SocioMap()
        {
            Map(x => x.Id).Ignore();
            Map(x => x.Nombre).Index(0);
            Map(x => x.Edad).Index(1);
            Map(x => x.Equipo).Index(2);
            Map(x => x.EstadoCivil).Index(3);
            Map(x => x.NivelDeEstudios).Index(4); 

        }
    }
}
