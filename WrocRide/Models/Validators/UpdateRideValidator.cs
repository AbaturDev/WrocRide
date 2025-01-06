using FluentValidation;

namespace WrocRide.Models.Validators
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
