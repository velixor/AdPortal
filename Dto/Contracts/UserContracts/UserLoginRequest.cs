using System.ComponentModel.DataAnnotations;

namespace Dto.Contracts.UserContracts
{
    public class UserLoginRequest : IRequest
    {
        [Required(ErrorMessage = "Email not specified")]
        public string Email { get; set; }
         
        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}