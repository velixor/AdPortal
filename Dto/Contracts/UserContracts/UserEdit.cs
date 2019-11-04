using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Contracts.UserContracts
{
    public class UserEdit : IResponse, IRequest
    {
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        [DisplayName("New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
         
        [DisplayName("Confirm password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password entered incorrectly")]
        public string ConfirmPassword { get; set; }
    }
}