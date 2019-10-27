using System;
using Microsoft.AspNetCore.Http;

namespace Dto.Contracts.AdContracts
{
    public class AdRequest : IAdRequest
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }
    }
}