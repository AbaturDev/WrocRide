
namespace WrocRide.API.Validators
{
    public class CreateRatingDtoValidator : AbstractValidator<CreateRatingDto>
    {
        public CreateRatingDtoValidator()
        {
            RuleFor(x => x.Grade)
                .NotEmpty()
                .InclusiveBetween(1, 5);

            RuleFor(x => x.Comment)
                .NotEmpty()
                .When(x => x.Comment != null)
                .MaximumLength(50);
        }
    }
}
