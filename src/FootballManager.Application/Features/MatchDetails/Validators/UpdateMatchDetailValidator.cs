using FluentValidation;
using FootballManager.Application.Features.MatchDetails.Commands.Update;

namespace FootballManager.Application.Features.MatchDetails.Validators
{
    public class UpdateMatchDetailValidator : AbstractValidator<UpdateMatchDetailCommand>
    {
        public UpdateMatchDetailValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                                         .GreaterThan(0);
            RuleFor(x => x.MemberId).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                                         .GreaterThan(0);
            RuleFor(x => x.MatchId).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                                         .GreaterThan(0);
            RuleFor(x => x.BibColour).NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
