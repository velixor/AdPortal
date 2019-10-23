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
        private readonly IOptions<StaticFilesOptions> _staticFilesOptions;

        public ImageHelper(IOptions<StaticFilesOptions> staticFilesOptions)
        {
            _staticFilesOptions = staticFilesOptions ?? throw new ArgumentNullException(nameof(staticFilesOptions));
        }

        private string GetPathOfImage(string imageName)
        {
            return Path.Combine(_staticFilesOptions.Value.StaticFilesPath,
                _staticFilesOptions.Value.ImagePath,
                imageName);
        }

        public async Task<string> UploadImageAndGetName(IFormFile image)
        {
            if (!(image?.Length > 0)) return null;

            var imageType = image.ContentType.Split('/')[1];
            var imageName = $"{Guid.NewGuid().ToString()}.{imageType}";
            var path = GetPathOfImage(imageName);

            await using var stream = new FileStream(path, FileMode.CreateNew);
            await image.CopyToAsync(stream);

            return imageName;
        }

        public void DeleteImage(string imageName)
        {
            var image = GetPathOfImage(imageName);
            if (File.Exists(image))
                File.Delete(image);
        }
    }
}