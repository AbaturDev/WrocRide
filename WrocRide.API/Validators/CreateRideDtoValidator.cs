namespace WrocRide.API.Validators
{
    public class CreateRideDtoValidator : AbstractValidator<CreateRideDto>
    {
        public CreateRideDtoValidator()
        {
            RuleFor(x => x.DriverId)
                .NotEmpty();

            RuleFor(x => x.Destination)
                .NotEmpty();

            RuleFor(x => x.PickUpLocation)
                .NotEmpty();
        }
    }
}
