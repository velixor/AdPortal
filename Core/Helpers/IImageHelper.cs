using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAndGetName(IFormFile image);
        void DeleteImage(string imageName);
    }
}