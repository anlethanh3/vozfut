using FluentValidation;
using FootballManager.Application.Features.MatchDetails.Commands.Create;

namespace FootballManager.Application.Features.MatchDetails.Validators
{
    public class CreateMatchDetailValidator : AbstractValidator<CreateMatchDetailCommand>
    {
        public CreateMatchDetailValidator()
        {
            RuleFor(x => x.MatchId).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                                   .GreaterThan(0);
            RuleFor(x => x.MemberId).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.BibColour).NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
