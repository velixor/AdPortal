using System;
using Dto.Contracts.AdContracts;
using FluentValidation;

namespace Core.Validators
{
    public class AdRequestValidator : AbstractValidator<AdRequest>
    {
        public AdRequestValidator()
        {
            RuleFor(ad => ad.Content).NotEmpty();
            RuleFor(ad => ad.UserId).Must(x => x != Guid.Empty);
            RuleFor(ad => ad.Image)
                .Must(x =>
                {
                    if (x == null)
                        return true;
                    var t = x.ContentType.Split('/');
                    return t.Length == 2 && t[0] == "image";
                });
        }
    }
}