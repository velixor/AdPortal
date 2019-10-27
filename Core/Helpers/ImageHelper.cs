using System;
using System.IO;
using System.Threading.Tasks;
using Core.Options;
using Dto.Contracts;
using Dto.Contracts.AdContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Core.Helpers
{
    public class ImageHelper : IImageHelper
    {
        private readonly IOptions<StaticFilesOptions> _staticFilesOptions;
        private readonly IHttpContextAccessor _contextAccessor;

        public ImageHelper(IOptions<StaticFilesOptions> staticFilesOptions, IHttpContextAccessor contextAccessor)
        {
            _staticFilesOptions = staticFilesOptions ?? throw new ArgumentNullException(nameof(staticFilesOptions));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }
        
        public async Task<string> UploadImageAndGetNameAsync(IFormFile image)
        {
            if (!(image?.Length > 0)) return null;

            var imageType = image.ContentType.Split('/')[1];
            var imageName = $"{Guid.NewGuid().ToString()}.{imageType}";
            var path = GetPathOfImage(imageName);

            await using var stream = new FileStream(path, FileMode.CreateNew);
            await image.CopyToAsync(stream);

            return imageName;
        }

        public void ImageNameToImageUrl(IHasImage ad)
        {
            if (ad == null) throw new ArgumentNullException(nameof(ad));
            if (ad.Image == null) return;

            ad.Image = GetImageUrl(ad.Image);
        }

        public void DeleteImage(string imageName)
        {
            if (imageName == null) return;

            var image = GetPathOfImage(imageName);
            if (File.Exists(image))
                File.Delete(image);
        }

        private string GetImageUrl(string imageName)
        {
            var url = $"{_contextAccessor.HttpContext.Request.Scheme}" +
                      $"://{_contextAccessor.HttpContext.Request.Host.ToUriComponent()}";
            return
                $"{url}" +
                $"/{_staticFilesOptions.Value.ImagePath}/" +
                $"{imageName}";
        }

        private string GetPathOfImage(string imageName)
        {
            return Path.Combine(_staticFilesOptions.Value.StaticFilesPath,
                _staticFilesOptions.Value.ImagePath,
                imageName);
        }
    }
}