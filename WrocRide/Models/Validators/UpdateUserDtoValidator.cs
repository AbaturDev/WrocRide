using FluentValidation;
using WrocRide.Entities;

namespace WrocRide.Models.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator(WrocRideDbContext dbContext)
        {
            RuleFor(x => x.Name)
                   .NotEmpty()
                   .MaximumLength(25);

            RuleFor(x => x.Surename)
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(e => e.Email == value);

                    if (emailInUse)
                    {
                        context.AddFailure("Email", "Email is already taken");
                    }
                });

            RuleFor(x => x.Password).MinimumLength(8);
            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);
        }
    }
}
