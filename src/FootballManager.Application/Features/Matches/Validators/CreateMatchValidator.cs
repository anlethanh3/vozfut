using FluentValidation;
using FootballManager.Application.Features.Matches.Commands.Create;

namespace FootballManager.Application.Features.Matches.Validators
{
    public class CreateMatchValidator : AbstractValidator<CreateMatchCommand>
    {
        public CreateMatchValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required.")
                                .MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(200);
            RuleFor(x => x.Teamsize).NotNull().WithMessage("{PropertyName} is required.")
                                    .Must(x => x > 0);
            RuleFor(x => x.TeamCount).NotNull().WithMessage("{PropertyName} is required.")
                                     .Must(x => x > 0);
        }
    }
}
