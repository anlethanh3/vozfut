using FluentValidation;
using FootballManager.Application.Features.MemberVotes.Commands.Create;

namespace FootballManager.Application.Features.MemberVotes.Validators
{
    public class CreateMemberVoteValidator : AbstractValidator<CreateOrUpdateMemberVoteCommand>
    {
        public CreateMemberVoteValidator()
        {
            RuleFor(x => x.MemberId).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.VoteId).NotNull().NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
