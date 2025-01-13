namespace WrocRide.API.Validators
{
    public class UpdateDocumentDtoValidator : AbstractValidator<UpdateDocumentDto>
    {
        public UpdateDocumentDtoValidator()
        {
            RuleFor(x => x.DocumentStatus)
                .NotEmpty()
                .IsInEnum();
        }
    }
}
