using FluentValidation;
using RedFox.Application.DTO;

namespace RedFox.Application.Validators
{
    public class GeoDtoValidator : AbstractValidator<GeoDto>
    {
        public GeoDtoValidator()
        {
            RuleFor(x => x.Lat)
                .NotEmpty().WithMessage("Latitude is required.");
            RuleFor(x => x.Lng)
                .NotEmpty().WithMessage("Longitude is required.");
        }
    }
}
