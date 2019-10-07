using System;

namespace AdPortalApi.Contracts.Responses
{
    public class AdResponse
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public int Rating { get; set; }
        public DateTime CreationDate { get; set; }
    }
}