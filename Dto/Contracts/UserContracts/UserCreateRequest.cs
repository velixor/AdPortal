namespace Dto.Contracts.UserContracts
{
    public class UserCreateRequest : IRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}