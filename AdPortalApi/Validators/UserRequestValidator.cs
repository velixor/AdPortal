using Dto.Contracts.UserContracts;
using FluentValidation;

namespace AdPortalApi.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches(@"^[a-zA-Z]+(?:[_-]?[a-zA-Z0-9])*$");
        }
    }
}