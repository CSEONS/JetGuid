namespace GGuid.Service
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    public class ImageUploader
    {
        public readonly string DefaultEventLogo = "~/images/eventDefaultLogo.png";
        private readonly IWebHostEnvironment _hostingEnvironment;


        public ImageUploader(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length <= 0)
            {
                throw new ArgumentException("Invalid image file");
            }

            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            var imageUrl = "/uploads/" + uniqueFileName; // URL для доступа к сохраненной картинке

            return imageUrl;
        }
    }
}
