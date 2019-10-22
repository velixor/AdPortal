using System;
using System.IO;
using AdPortalApi.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AdPortalApi.Services
{
    public class ImageService : IImageService
    {
        private readonly IOptions<ImageConfigs> _imageConfigs;

        public ImageService(IOptions<ImageConfigs> imageConfigs)
        {
            _imageConfigs = imageConfigs ?? throw new ArgumentNullException(nameof(imageConfigs));
        }

        private string GetRelPath(string imageName)
            => Path.Combine(_imageConfigs.Value.Path, imageName);

        public string UploadImageAndGetName(IFormFile image)
        {
            if (!(image?.Length > 0)) return null;
            var imageType = image.ContentType.Split('/')[1];
            var imageName = $"{Guid.NewGuid().ToString()}.{imageType}";
            var path = GetRelPath(imageName);
            using (var stream = new FileStream(path, FileMode.CreateNew))
            {
                image.CopyTo(stream);
            }
            return imageName;
        }
    }
}