namespace WrocRide.API.Validators
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
