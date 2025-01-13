﻿namespace WrocRide.API.Validators
{
    public class DriverQueryValidator : AbstractValidator<DriverQuery>
    {
        private readonly int[] allowedPageSizes = new int[] { 5, 10, 15, 20 };
        public DriverQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .Custom((value, context) =>
                {
                    if(!allowedPageSizes.Contains(value))
                    {
                        context.AddFailure($"Wrong page size value. Page size must be in {string.Join(",", allowedPageSizes)}");
                    }
                });

            RuleFor(x => x.DriverStatus).IsInEnum();
        }

    }
}
