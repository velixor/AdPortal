using System.IO;
using Microsoft.AspNetCore.Http;

namespace AdPortalApi.Services
{
    public interface IImageService
    {
        string UploadImageAndGetName(IFormFile image);
    }
}