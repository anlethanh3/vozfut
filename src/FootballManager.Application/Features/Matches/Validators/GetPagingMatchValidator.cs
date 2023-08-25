using FluentValidation;
using FootballManager.Application.Features.Matches.Queries.GetPaging;

namespace FootballManager.Application.Features.Matches.Validators
{
    public class GetPagingMatchValidator : AbstractValidator<GetPagingMatchQuery>
    {
        public GetPagingMatchValidator()
        {
            RuleFor(x => x.Page).NotNull().WithMessage("{PropertyName} is required.")
                                .Must(x => x >= 0);
            RuleFor(x => x.Limit).NotNull().WithMessage("{PropertyName} is required.")
                                 .Must(x => x >= 0);
            RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate).When(x => x.StartDate != null);
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate != null && x.StartDate != null);
        }
    }
}
