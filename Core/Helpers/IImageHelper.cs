using System.Threading.Tasks;
using Dto.Contracts;
using Dto.Contracts.AdContracts;
using Microsoft.AspNetCore.Http;

namespace Core.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAndGetNameAsync(IFormFile image);
        void ImageNameToImageUrl(IHasImage hasImage);
        void DeleteImage(string imageName);
    }
}