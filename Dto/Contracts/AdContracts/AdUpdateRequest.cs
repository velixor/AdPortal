using System;
using Microsoft.AspNetCore.Http;

namespace Dto.Contracts.AdContracts
{
    public class AdUpdateRequest : IRequest
    {
        public Guid Guid { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }
    }
}