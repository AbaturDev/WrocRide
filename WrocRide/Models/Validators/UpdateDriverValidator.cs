using FluentValidation;

namespace WrocRide.Models.Validators
{
    public class UpdateDriverPricingDtoValidator : AbstractValidator<UpdateDriverPricingDto>
    {
        public UpdateDriverPricingDtoValidator()
        {
            RuleFor(x => x.Pricing)
                .NotEmpty()
                .GreaterThan(0);
        }
    }

    public class UpdateDriverStatusDtoValidator : AbstractValidator<UpdateDriverStatusDto>
    {
        public UpdateDriverStatusDtoValidator()
        {
            RuleFor(x => x.DriverStatus)
                .NotEmpty()
                .IsInEnum();
        }
    }
}
