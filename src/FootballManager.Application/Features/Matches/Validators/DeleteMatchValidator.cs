using FluentValidation;
using FootballManager.Application.Features.Matches.Commands.Delete;

namespace FootballManager.Application.Features.Matches.Validators
{
    public class DeleteMatchValidator : AbstractValidator<DeleteMatchCommand>
    {
        public DeleteMatchValidator()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("{PropertyName} is required.")
                            .Must(x => x > 0);
        }
    }
}
