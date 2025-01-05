using FluentValidation;

namespace WrocRide.Models.Validators
{
    public class CreateReportDtoValidator : AbstractValidator<CreateReportDto>
    {
        public CreateReportDtoValidator() 
        {
            RuleFor(x => x.RideId)
                .NotEmpty();

            RuleFor(x => x.Reason)
                .NotEmpty()
                .When(x => x.Reason != null)
                .MinimumLength(20);

            RuleFor(x => x.ReportedId)
                .NotEmpty();
        }
    }
}
