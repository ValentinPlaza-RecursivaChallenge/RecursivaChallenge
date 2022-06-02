using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text;

namespace RecursivaChallenge.Helper
{
    public class Files
    {
        public static async Task<string> SaveAsync(IFormFile file)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), file.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return filePath;
        }

        public static void Delete(string filePath)
        {
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }

        public static bool FormatoCorrecto<T, TMap>(string filePath) where TMap : ClassMap
        {
            try
            {
                var culture = new CultureInfo("es-AR");
                culture.TextInfo.ListSeparator = ";";

                using (var reader = new StreamReader(filePath, Encoding.UTF7))
                using (var csv = new CsvReader(reader, culture))
                {
                    var columns = reader.ReadLine().Split(';');
                    if (columns.Count() != 5)
                        return false;

                    csv.Context.RegisterClassMap<TMap>();
                    csv.Read();
                    var socios = csv.GetRecord<T>();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}