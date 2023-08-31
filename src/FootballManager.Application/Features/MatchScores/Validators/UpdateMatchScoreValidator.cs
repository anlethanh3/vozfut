using FluentValidation;
using FootballManager.Application.Features.MatchScores.Commands.Update;

namespace FootballManager.Application.Features.MatchScores.Validators
{
    public class UpdateMatchScoreValidator : AbstractValidator<UpdateMatchScoreCommand>
    {
        public UpdateMatchScoreValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.NumberGoalTeam1).NotNull().WithMessage("{PropertyName} is required.")
                                           .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must greate than or equal zero");
            RuleFor(x => x.NumberGoalTeam2).NotNull().WithMessage("{PropertyName} is required.")
                                           .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must greate than or equal zero");
        }
    }
}
