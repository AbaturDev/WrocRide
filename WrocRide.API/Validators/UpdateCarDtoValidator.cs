namespace WrocRide.API.Validators
{
    public class UpdateCarDtoValidator : AbstractValidator<UpdateCarDto>
    {
        public UpdateCarDtoValidator()
        {
            RuleFor(c => c.LicensePlate)
                .NotEmpty()
                .When(c => c.LicensePlate != null);

            RuleFor(c => c.Brand)
                .NotEmpty()
                .When(c => c.Brand != null);

            RuleFor(c => c.Model)
                .NotEmpty()
                .When(c => c.Model != null);

            RuleFor(c => c.BodyColor)
                .NotEmpty()
                .When(c => c.BodyColor != null);

            RuleFor(x => x.YearProduced)
                .InclusiveBetween(2000, DateTime.Now.Year)
                .When(x => x.YearProduced.HasValue)
                .WithMessage($"YearProduced must be beetween 2000-{DateTime.Now.Year}");
        }
    }
}
