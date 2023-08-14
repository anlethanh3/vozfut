using FluentValidation;
using FootballManager.Application.Features.Matches.Commands.Update;

namespace FootballManager.Application.Features.Matches.Validators
{
    public class UpdateMatchValidator : AbstractValidator<UpdateMatchCommand>
    {
        public UpdateMatchValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required.")
                                         .MaximumLength(50);
            RuleFor(x => x.Description).MaximumLength(200);
            RuleFor(x => x.TeamCount).Must(x => x > 0);
            RuleFor(x => x.TeamSize).Must(x => x > 0);
        }
    }
}
