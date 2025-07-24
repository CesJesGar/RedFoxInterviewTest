using FluentValidation;
using RedFox.Application.DTO;

namespace RedFox.Application.Validators
{
    public class GeoDtoValidator : AbstractValidator<GeoDto>
    {
        public GeoDtoValidator()
        {
            RuleFor(x => x.lat)
                .NotEmpty().WithMessage("Latitude is required.");
            RuleFor(x => x.lng)
                .NotEmpty().WithMessage("Longitude is required.");
        }
    }
}
