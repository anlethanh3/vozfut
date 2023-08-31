using FluentValidation;
using FootballManager.Application.Features.MatchScores.Commands.Create;

namespace FootballManager.Application.Features.MatchScores.Validators
{
    public class CreateMatchScoreValidator : AbstractValidator<CreateMatchScoreCommand>
    {
        public CreateMatchScoreValidator()
        {
            RuleFor(x => x.MatchId).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                                 .GreaterThan(0).WithMessage("{PropertyName} must greater than zero");
        }
    }
}
