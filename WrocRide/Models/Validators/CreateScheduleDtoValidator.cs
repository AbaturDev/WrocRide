using FluentValidation;

namespace WrocRide.Models.Validators;

public class CreateScheduleDtoValidator : AbstractValidator<CreateScheduleDto>
{
    public CreateScheduleDtoValidator()
    {
        RuleFor(x => x.PickUpLocation).NotEmpty();

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Start time must be in the format 'hh:mm:ss'");
        
        RuleFor(x => x.Destination).NotEmpty();
        
        RuleFor(x => x.DayOfWeekIds).NotEmpty();
    }
}