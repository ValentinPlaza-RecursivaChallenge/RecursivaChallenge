using Microsoft.AspNetCore.Mvc;
using RecursivaChallenge.Helper;
using RecursivaChallenge.Models.Entity;
using RecursivaChallenge.Models.ResponseObject;
using RecursivaChallenge.Repository.Contract;

namespace RecursivaChallenge.Controllers
{
    public class SocioController : Controller
    {
        private readonly ISociosRepository repository;

        public ILogger<SocioController> Logger { get; }

        public SocioController(ISociosRepository repository, ILogger<SocioController> logger)
        {
            this.repository = repository;
            Logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            try
            {
                var guardo = await GuardarEnBDCsv(file);
                if (guardo)
                    ViewBag.SocioResponse = await CargarResponse();

                return View();

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Algo salio mal al procesar el archivo");
                RedirectToAction("Index", "Socio");
                throw;
            }
        }

        private async Task<bool> GuardarEnBDCsv(IFormFile file)
        {
            try
            {
                var borro = await repository.TruncateTabla("Socios");
                var filePath = await Files.SaveAsync(file);
                var guardo = await repository.InsertBulkFileCsv(filePath);
                Files.Delete(filePath);
                return guardo;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Algo salio mal al guardar archivo csv");
                throw ex;
            }
        }

        private async Task<SocioResponse> CargarResponse()
        {
            try
            {
                var socioResponse = new SocioResponse();

                var SocioXEsYNvTask = repository.SocioXEstadoCivilYNivelDeEstudio("Casado", "Universitario");
                var SocioXEsYNv = await SocioXEsYNvTask;
                var promedioEdadClubTask = repository.PromedioEdadClub("Racing");
                var promedioEdadClub = await promedioEdadClubTask;
                var nombresMasComunesTask = repository.NombresMasComunesXClub("River");
                var nombresMasComunes = await nombresMasComunesTask;
                var clubsInfoTask = repository.SocioXClubYInfoEdades();
                var clubInfo = await clubsInfoTask;
                var cantTotalTask = repository.CantidadTotalSocio();
                var cantTotal = await cantTotalTask;


                socioResponse.NombresComunes = nombresMasComunes;
                socioResponse.PromedioEdadClub = promedioEdadClub;
                socioResponse.CantidadTotal = cantTotal;
                socioResponse.ClubsInfo = clubInfo;
                socioResponse.SocioInfoParcial = SocioXEsYNv;

                return socioResponse;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error al cargar Socio Response");
                throw ex;
            }
        }
    }
}