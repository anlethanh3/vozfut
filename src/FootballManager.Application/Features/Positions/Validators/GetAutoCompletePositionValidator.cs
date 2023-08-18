using FluentValidation;
using FootballManager.Application.Features.Positions.Queries.GetAutoComplete;

namespace FootballManager.Application.Features.Positions.Validators
{
    public class GetAutoCompletePositionValidator : AbstractValidator<GetAutoCompletePositionQuery>
    {
        public GetAutoCompletePositionValidator()
        {
            RuleFor(x => x.Search).MaximumLength(20).WithMessage("{propertyName} have maximum length 20");
        }
    }
}
