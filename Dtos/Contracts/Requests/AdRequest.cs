using System;

namespace Dtos.Contracts.Requests
{
    public class AdRequest
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
    }
}