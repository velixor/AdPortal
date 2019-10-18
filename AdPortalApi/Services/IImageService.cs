using System.IO;
using Microsoft.AspNetCore.Http;

namespace AdPortalApi.Services
{
    public interface IImageService
    {
        string UploadImage(IFormFile image);
        Image GetImage(string imageName);
    }
}