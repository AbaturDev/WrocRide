namespace WrocRide.API.Validators;

public class AddCreditsDtoValidator : AbstractValidator<AddCreditsDto>
{
    public AddCreditsDtoValidator()
    {
        RuleFor(x => x.Credits)
            .NotEmpty()
            .GreaterThan(0);
    }
}