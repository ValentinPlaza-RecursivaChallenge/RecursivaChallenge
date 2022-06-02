using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using RecursivaChallenge.CsvMap;
using RecursivaChallenge.Helper;
using RecursivaChallenge.Models.Entity;
using RecursivaChallenge.Models.ResponseObject;
using RecursivaChallenge.Models.ViewModels;
using RecursivaChallenge.Repository.Contract;
using System.Globalization;
using System.Text;

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
        public async Task<IActionResult> Index(SociosViewModel sociosViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(sociosViewModel);


                var guardo = await GuardarEnBDCsv(sociosViewModel.CsvFile);

                if (guardo)
                    ViewBag.SocioResponse = CargarResponse();

                return View();

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Algo salio mal al procesar el archivo");
                return View();
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
                var valido = Files.FormatoCorrecto<Socio, SocioMap>(filePath);

                if (!valido)
                {
                    ViewBag.MalFormateado = "El Formato del archivo es incorrecto.";
                    Files.Delete(filePath);
                    return false;
                }
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