using System.Text.RegularExpressions;

namespace WrocRide.API.Validators
{
    public class RegisterDriverDtoValidator : AbstractValidator<RegisterDriverDto>
    {
        public RegisterDriverDtoValidator(WrocRideDbContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(x => x.Surename)
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(new Regex(@"^\d{9}$")).WithMessage("PhoneNumber not valid");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.RoleId)
                .NotEmpty()
                .InclusiveBetween(1, 3);

            RuleFor(x => x.Password).MinimumLength(8);
            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(e => e.Email == value);

                    if (emailInUse)
                    {
                        context.AddFailure("Email", "Email is already taken");
                    }
                });

            RuleFor(x => x.FileLocation)
                .NotEmpty();

            RuleFor(x => x.LicensePlate)
                .NotEmpty();

            RuleFor(x => x.Brand)
                .NotEmpty();

            RuleFor(x => x.Model)
                .NotEmpty();

            RuleFor(x => x.BodyColor)
                .NotEmpty();

            RuleFor(x => x.YearProduced)
                .NotEmpty()
                .InclusiveBetween(2000, DateTime.Now.Year);

            RuleFor(x => x.Pricing)
                 .NotEmpty()
                 .GreaterThan(0);
        }
    }
}
