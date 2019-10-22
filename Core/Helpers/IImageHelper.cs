using Microsoft.AspNetCore.Http;

namespace Core.Helpers
{
    public interface IImageHelper
    {
        string UploadImageAndGetName(IFormFile image);
    }
}