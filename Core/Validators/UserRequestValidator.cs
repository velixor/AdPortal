using Dto.Contracts.UserContracts;
using FluentValidation;

namespace Core.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$");
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}