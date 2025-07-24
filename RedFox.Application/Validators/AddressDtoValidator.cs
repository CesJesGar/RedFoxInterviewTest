using FluentValidation;
using RedFox.Application.DTO;

namespace RedFox.Application.Validators
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.street).NotEmpty().WithMessage("Street is required.");
            RuleFor(x => x.suite).NotEmpty().WithMessage("Suite is required.");
            RuleFor(x => x.city).NotEmpty().WithMessage("City is required.");
            RuleFor(x => x.zipcode).NotEmpty().WithMessage("Zipcode is required.");
            RuleFor(x => x.geo)
                .NotNull().WithMessage("Geo is required.")
                .SetValidator(new GeoDtoValidator());
        }
    }
}
