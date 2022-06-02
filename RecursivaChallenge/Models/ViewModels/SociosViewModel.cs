using RecursivaChallenge.Validations;
using System.ComponentModel.DataAnnotations;

namespace RecursivaChallenge.Models.ViewModels
{
    public class SociosViewModel
    {
        [Required(ErrorMessage = "Por favor, seleccione un archivo.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(100_000_000)]
        [AllowedExtensions(new string[] { ".csv" })]
        public IFormFile CsvFile { get; set; }
    }
}
