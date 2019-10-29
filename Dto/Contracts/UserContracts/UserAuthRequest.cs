namespace Dto.Contracts.UserContracts
{
    public class UserAuthRequest : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}