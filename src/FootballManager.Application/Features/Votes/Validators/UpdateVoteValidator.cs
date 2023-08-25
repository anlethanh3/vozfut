using FluentValidation;
using FootballManager.Application.Features.Votes.Commands.Update;

namespace FootballManager.Application.Features.Votes.Validators
{
    public class UpdateVoteValidator : AbstractValidator<UpdateVoteCommand>
    {
        public UpdateVoteValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required.").MaximumLength(100).WithMessage("{PropertyName} has max length 100");
            RuleFor(x => x.Description).MaximumLength(200).WithMessage("{PropertyName} has max length 200");
        }
    }
}
