namespace WrocRide.API.Validators
{
    public class RideQueryValidator : AbstractValidator<RideQuery>
    {
        private readonly int[] allowedPageSizes = new int[] { 5, 10, 15, 20 };

        public RideQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .Custom((value, context) =>
                {
                    if (!allowedPageSizes.Contains(value))
                    {
                        context.AddFailure($"Wrong page size value. Page size must be in {string.Join(",", allowedPageSizes)}");
                    }
                });

            RuleFor(x => x.RideStatus)
                .IsInEnum()
                .When(x => x.RideStatus != null);
        }
    }
}
