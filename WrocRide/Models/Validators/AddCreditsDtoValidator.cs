using FluentValidation;

namespace WrocRide.Models.Validators;

public class AddCreditsDtoValidator : AbstractValidator<AddCreditsDto>
{
    public AddCreditsDtoValidator()
    {
        RuleFor(x => x.Credits)
            .NotEmpty()
            .GreaterThan(0);
    }
}