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
                    ViewBag.SocioResponse = CargarResponse();

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

                if (!borro)
                    return false;

                var filePath = await Files.SaveAsync(file);
                var guardo = await repository.InsertBulkFileCsv(filePath);
                Files.Delete(filePath);
                return guardo;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Algo salio mal al guardar archivo csv");
                return false;
            }
        }

        private SocioResponse CargarResponse()
        {
            try
            {
                var socioResponse = new SocioResponse();

                socioResponse.NombresComunes = repository.NombresMasComunesXClub("River");
                socioResponse.PromedioEdadClub = repository.PromedioEdadClub("Racing");
                socioResponse.CantidadTotal = repository.CantidadTotalSocio();
                socioResponse.ClubsInfo = repository.SocioXClubYInfoEdades();
                socioResponse.SocioInfoParcial = repository.SocioXEstadoCivilYNivelDeEstudio("Casado", "Universitario");

                return socioResponse;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error al cargar Socio Response");
                return null;
            }
        }
    }
}