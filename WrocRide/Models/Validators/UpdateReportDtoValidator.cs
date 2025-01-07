using FluentValidation;

namespace WrocRide.Models.Validators
{
    public class UpdateReportDtoValidator : AbstractValidator<UpdateReportDto>
    {
        public UpdateReportDtoValidator()
        {
            RuleFor(x => x.ReportStatus)
                .NotEmpty()
                .IsInEnum();
        }
    }
}
