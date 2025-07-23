using FluentValidation;
using RedFox.Application.DTO;

namespace RedFox.Application.Validators
{
    public class UserCreationDtoValidator : AbstractValidator<UserCreationDto>
    {
        public UserCreationDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required.");
            RuleFor(x => x.Website).NotEmpty().WithMessage("Website is required.");
            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address is required.")
                .SetValidator(new AddressDtoValidator());
            RuleFor(x => x.Company)
                .NotNull().WithMessage("Company is required.")
                .SetValidator(new CompanyDtoValidator());
        }
    }
}
