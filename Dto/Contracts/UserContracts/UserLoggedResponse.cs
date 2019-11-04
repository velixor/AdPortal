using System;

namespace Dto.Contracts.UserContracts
{
    public class UserLoggedResponse : IResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}