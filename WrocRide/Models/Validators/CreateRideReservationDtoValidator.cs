using FluentValidation;

namespace WrocRide.Models.Validators;

public class CreateRideReservationDtoValidator : AbstractValidator<CreateRideReservationDto>
{
    public CreateRideReservationDtoValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThan(x => DateTime.Now.AddHours(1));
    }
}