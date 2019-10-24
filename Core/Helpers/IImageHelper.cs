using System.Threading.Tasks;
using Dto.Contracts.AdContracts;
using Microsoft.AspNetCore.Http;

namespace Core.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAndGetName(IFormFile image);
        void ImageNameToImageUrl(AdResponse ad);
        void DeleteImage(string imageName);
    }
}