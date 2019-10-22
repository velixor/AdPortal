using Microsoft.AspNetCore.Http;

namespace Core
{
    public interface IImageHelper
    {
        string UploadImageAndGetName(IFormFile image);
    }
}