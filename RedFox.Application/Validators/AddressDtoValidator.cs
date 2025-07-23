using FluentValidation;
using RedFox.Application.DTO;

namespace RedFox.Application.Validators
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.Street).NotEmpty().WithMessage("Street is required.");
            RuleFor(x => x.Suite).NotEmpty().WithMessage("Suite is required.");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");
            RuleFor(x => x.Zipcode).NotEmpty().WithMessage("Zipcode is required.");
            RuleFor(x => x.Geo)
                .NotNull().WithMessage("Geo is required.")
                .SetValidator(new GeoDtoValidator());
        }
    }
}
