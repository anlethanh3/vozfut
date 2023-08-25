using FluentValidation;
using FootballManager.Application.Features.Votes.Commands.Delete;

namespace FootballManager.Application.Features.Votes.Validators
{
    public class DeleteVoteValidator : AbstractValidator<DeleteVoteCommand>
    {
        public DeleteVoteValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
        }
    }
}
