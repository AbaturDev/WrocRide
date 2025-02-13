using WrocRide.Shared.PaginationHelpers;

namespace WrocRide.API.Validators
{
    public class PageQueryValidator : AbstractValidator<PageQuery>
    {
        private readonly int[] allowedPageSizes = new int[] { 5, 10, 15, 20 };
        public PageQueryValidator()
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
        }
    }
}