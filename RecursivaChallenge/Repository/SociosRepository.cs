using Microsoft.EntityFrameworkCore;
using RecursivaChallenge.Data;
using RecursivaChallenge.Models.ResponseObject;
using RecursivaChallenge.Repository.Contract;

namespace RecursivaChallenge.Repository
{
    public class SociosRepository : ISociosRepository
    {
        private readonly ApplicationDbContext context;

        public ILogger<SociosRepository> Logger { get; }

        public SociosRepository(ApplicationDbContext context, ILogger<SociosRepository> logger)
        {
            this.context = context;
            Logger = logger;
        }

        public async Task<List<ClubInfoResponse>> SocioXClubYInfoEdades()
        {
            try
            {
                var rta = context.Socios
                   .GroupBy(x => x.Equipo)
                   .OrderByDescending(x => x.Count())
                   .Select(x => new ClubInfoResponse
                   {
                       Equipo = x.Key,
                       Cantidad = x.Count(),
                       PromedioEdad = Math.Round(x.Average(y => y.Edad), 2),
                       EdadMinima = x.Min(y => y.Edad),
                       EdadMaxima = x.Max(y => y.Edad)
                   })
                   .ToList();

                return rta;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Algo salio mal en repository");
                throw;
            }
        }

        public async Task<List<string>> NombresMasComunesXClub(string club)
        {
            try
            {
                var rta = context.Socios
                    .Where(x => x.Equipo == club)
                    .GroupBy(x => x.Nombre)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Key)
                    .Take(5)
                    .ToList();
                return rta;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Surgio un error al buscar los nombre comunes por club");
                return new List<string>();
            }
        }
        public async Task<List<SocioInfoParcialResponse>> SocioXEstadoCivilYNivelDeEstudio(string estadoCivil, string nivelDeEstudios)
        {
            try
            {
                var rta = context.Socios
                    .OrderBy(x => x.Edad)
                    .Where(x => x.EstadoCivil == estadoCivil && x.NivelDeEstudios == nivelDeEstudios)
                    .Select(x => new SocioInfoParcialResponse{ Nombre = x.Nombre, Edad = x.Edad, Equipo = x.Equipo })
                    .Take(100)
                    .ToList();
                return rta;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Algo salio mal en consulta SocioXEstadoYNivelDeEstudio");
                return new List<SocioInfoParcialResponse>();
            }
        }
        public async Task<double?> PromedioEdadClub(string club)
        {
            try
            {
                var promedio = context.Socios
                    .Where(x => x.Equipo == club)
                    .Average(x => x.Edad);

                return promedio;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Surgio un error al calcular el promedio de edad");
                return null;
            }
        }
        public async Task<int> CantidadTotalSocio()
        {
            var rta = context.Socios.Count();
            return rta;
        }
        public async Task<bool> InsertBulkFileCsv(string filePath)
        {
            try
            {
                var formatFilePath = AppContext.BaseDirectory + @"FormatFileSql.xml";
                await context.Database.ExecuteSqlInterpolatedAsync($"EXEC [dbo].[sp_InsertBulkCsv] {filePath}, {formatFilePath}");

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Surgio un error al insertar csv en la BD");
                return false;
            }
        }
        public async Task<bool> TruncateTabla(string nombreTabla)
        {
            try
            {
                await context.Database.ExecuteSqlInterpolatedAsync($"EXEC [dbo].[sp_TruncateTable] {nombreTabla}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error al truncar la tabla Socio");
                return false;
            }
        }
    }
}