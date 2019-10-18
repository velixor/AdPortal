using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace AdPortalApi.Services
{
    public class ImageService : IImageService
    {
        private readonly IHostEnvironment _hostEnvironment;

        public ImageService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        private string GetRelPath(string imageName)
            => Path.Combine("wwwroot", "Images", imageName);

        public string UploadImage(IFormFile image)
        {
            if (!(image?.Length > 0)) return string.Empty;
            var imageType = image.ContentType.Split('/')[1];
            var imageName = $"{Guid.NewGuid().ToString()}.{imageType}";
            var path = GetRelPath(imageName);
            using (var stream = new FileStream(path, FileMode.CreateNew))
            {
                image.CopyTo(stream);
            }

            return imageName;
        }

        public Image GetImage(string imageName)
        {
            var contentType = $"image/{imageName.Split('.')[1]}";
            var bytes = File.ReadAllBytes(GetRelPath(imageName));
            return new Image{Bytes = bytes, ContentType = contentType};
        }
    }

    public class Image
    {
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
    }
}