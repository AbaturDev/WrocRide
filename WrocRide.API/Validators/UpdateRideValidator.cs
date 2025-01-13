namespace WrocRide.API.Validators
{
    public class UpdateRideStatusDtoValidator : AbstractValidator<UpdateRideStatusDto>
    {
        public UpdateRideStatusDtoValidator()
        {
            RuleFor(x => x.RideStatus)
                .NotEmpty()
                .IsInEnum();
        }
    }
}
