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

    public class RideDriverDecisionDtoValidator : AbstractValidator<RideDriverDecisionDto>
    {
        public RideDriverDecisionDtoValidator()
        {
            RuleFor(x => x.RideStatus)
                .NotEmpty()
                .IsInEnum();

            RuleFor(x => x.Coast)
                .GreaterThan(0);
        }
    }
}
