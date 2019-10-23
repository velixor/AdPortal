using System;
using System.IO;
using System.Threading.Tasks;
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

        private string GetRelPath(string imageName) => Path.Combine(_imageConfigs.Value.Path, imageName);

        public async Task<string> UploadImageAndGetName(IFormFile image)
        {
            if (!(image?.Length > 0)) return null;
            
            var imageType = image.ContentType.Split('/')[1];
            var imageName = $"{Guid.NewGuid().ToString()}.{imageType}";
            var path = GetRelPath(imageName);

            await using var stream = new FileStream(path, FileMode.CreateNew);
            await image.CopyToAsync(stream);
            
            return imageName;
        }
    }
}