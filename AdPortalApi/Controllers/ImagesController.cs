using System;
using AdPortalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdPortalApi.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("{image}")]
        public IActionResult Get(string image)
        {
            var img = _imageService.GetImage(image);
            return File(img.Bytes, img.ContentType);
        }
    }
}