using FluentValidation;
using FootballManager.Application.Features.Positions.Queries.GetPaging;

namespace FootballManager.Application.Features.Positions.Validators
{
    public class GetPagingPositionValidator : AbstractValidator<GetPagingPositionQuery>
    {
        public GetPagingPositionValidator()
        {
            RuleFor(x => x.Page).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                              .Must(x => x >= 0).WithMessage("{PropertyName} must have greater or equal zero");
            RuleFor(x => x.Limit).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                              .Must(x => x >= 0).WithMessage("{PropertyName} must have greater or equal zero");
            RuleFor(x => x.Search).MaximumLength(20).WithMessage("{PropertyName} have maximum length 20");
            RuleFor(x => x.Sort).MaximumLength(20).WithMessage("{PropertyName} have maximum length 20");
        }
    }
}
