using FluentValidation;
using FootballManager.Application.Features.Votes.Commands.Create;

namespace FootballManager.Application.Features.Votes.Validators
{
    public class CreateVoteValidator : AbstractValidator<CreateVoteCommand>
    {
        public CreateVoteValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                                .MaximumLength(200).WithMessage("{PropertyName} has max length is 200");
            RuleFor(x => x.Description).MaximumLength(200).WithMessage("{PropertyName} have max length 200");
        }
    }
}
