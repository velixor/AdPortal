using System;

namespace Dto.Contracts.AdContracts
{
    public class AdResponse : IAdResponse, IHasImage
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public int Rating { get; set; }
        public DateTime CreationDate { get; set; }
    }
}