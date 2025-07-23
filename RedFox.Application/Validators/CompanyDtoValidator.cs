using FluentValidation;
using RedFox.Application.DTO;

namespace RedFox.Application.Validators
{
    public class CompanyDtoValidator : AbstractValidator<CompanyDto>
    {
        public CompanyDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Company name is required.");
            RuleFor(x => x.CatchPhrase).NotEmpty().WithMessage("CatchPhrase is required.");
            RuleFor(x => x.Bs).NotEmpty().WithMessage("Bs is required.");
        }
    }
}
