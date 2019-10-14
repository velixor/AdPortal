using Dto.Contracts.AdContracts;
using FluentValidation;

namespace AdPortalApi.Validators
{
    public class AdRequestValidator : AbstractValidator<AdRequest>
    {
        public AdRequestValidator()
        {
            RuleFor(ad => ad.Content).NotEmpty();
            RuleFor(ad => ad.UserId).NotNull();
        }
    }
}