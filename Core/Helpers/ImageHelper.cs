using System;
using System.IO;
using Core.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Core.Helpers
{
    public class ImageHelper : IImageHelper
    {
        private readonly IOptions<ImageOptions> _imageConfigs;

        public ImageHelper(IOptions<ImageOptions> imageConfigs)
        {
            _imageConfigs = imageConfigs ?? throw new ArgumentNullException(nameof(imageConfigs));
        }

        private string GetRelPath(string imageName)
        {
            return Path.Combine(_imageConfigs.Value.Path, imageName);
        }

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