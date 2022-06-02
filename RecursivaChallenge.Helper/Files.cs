using Microsoft.AspNetCore.Http;

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

    }
}