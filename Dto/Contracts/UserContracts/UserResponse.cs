using System;

namespace Dto.Contracts.UserContracts
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int AdsCount { get; set; }
    }
}