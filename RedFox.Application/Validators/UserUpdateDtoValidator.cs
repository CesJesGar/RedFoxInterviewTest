using FluentValidation;
using RedFox.Application.DTO;

namespace RedFox.Application.Validators
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address is required.")
                .SetValidator(new AddressDtoValidator());

            RuleFor(x => x.Company)
                .NotNull().WithMessage("Company is required.")
                .SetValidator(new CompanyDtoValidator());
        }
    }
}
