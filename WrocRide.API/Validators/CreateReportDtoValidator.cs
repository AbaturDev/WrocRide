﻿namespace WrocRide.API.Validators
{
    public class CreateReportDtoValidator : AbstractValidator<CreateReportDto>
    {
        public CreateReportDtoValidator() 
        {
            RuleFor(x => x.Reason)
                .NotEmpty()
                .When(x => x.Reason != null)
                .MinimumLength(20);
        }
    }
}
