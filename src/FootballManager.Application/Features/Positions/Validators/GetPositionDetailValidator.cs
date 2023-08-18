using FluentValidation;
using FootballManager.Application.Features.Positions.Queries.GetDetail;

namespace FootballManager.Application.Features.Positions.Validators
{
    public class GetPositionDetailValidator : AbstractValidator<GetDetailPositionQuery>
    {
        public GetPositionDetailValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                            .Must(x => x > 0);
        }
    }
}
