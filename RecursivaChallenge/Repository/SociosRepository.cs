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

        public List<ClubInfoResponse> SocioXClubYInfoEdades()
        {
            try
            {
                return context.Socios
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
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Algo salio mal en repository");
                throw;
            }
        }

        public List<string> NombresMasComunesXClub(string club)
        {
            try
            {
                return context.Socios
                    .Where(x => x.Equipo == club)
                    .GroupBy(x => x.Nombre)
                    .OrderByDescending(x => x.Count())
                    .Take(5)
                    .Select(x => x.Key)
                    .ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Surgio un error al buscar los nombre comunes por club");
                return new List<string>();
            }
        }
        public List<SocioInfoParcialResponse> SocioXEstadoCivilYNivelDeEstudio(string estadoCivil, string nivelDeEstudios)
        {
            try
            {
                return context.Socios
                    .Where(x => x.EstadoCivil == estadoCivil && x.NivelDeEstudios == nivelDeEstudios)
                    .OrderBy(x => x.Edad)
                    .Take(100)
                    .Select(x => new SocioInfoParcialResponse{ Nombre = x.Nombre, Edad = x.Edad, Equipo = x.Equipo })
                    .ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Algo salio mal en consulta SocioXEstadoYNivelDeEstudio");
                return new List<SocioInfoParcialResponse>();
            }
        }
        public double? PromedioEdadClub(string club)
        {
            try
            {
                return context.Socios
                    .Where(x => x.Equipo == club)
                    .Average(x => x.Edad);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Surgio un error al calcular el promedio de edad");
                return null;
            }
        }
        public int CantidadTotalSocio()
        {
            try
            {
                return context.Socios.Count();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Surgio un error al obtener la Cantidad totald de socios");
                return 0;
            }
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