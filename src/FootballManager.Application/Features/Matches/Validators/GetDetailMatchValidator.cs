using FluentValidation;
using FootballManager.Application.Features.Matches.Queries.GetDetail;

namespace FootballManager.Application.Features.Matches.Validators
{
    public class GetDetailMatchValidator : AbstractValidator<GetDetailMatchDto>
    {
        public GetDetailMatchValidator()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("{PropertyName} is required.")
                            .Must(x => x > 0);
        }
    }
}
