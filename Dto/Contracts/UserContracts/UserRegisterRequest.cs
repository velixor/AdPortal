using System.ComponentModel.DataAnnotations;

namespace Dto.Contracts.UserContracts
{
    public class UserRegisterRequest : IRequest
    {
        public string Name { get; set; }
        [Required(ErrorMessage ="Email not specified")]
        public string Email { get; set; }
         
        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
         
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password entered incorrectly")]
        public string ConfirmPassword { get; set; }
    }
}