namespace WrocRide.API.Validators;

public class CreateRideReservationDtoValidator : AbstractValidator<CreateRideReservationDto>
{
    public CreateRideReservationDtoValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThan(x => DateTime.Now.AddHours(1));
    }
}