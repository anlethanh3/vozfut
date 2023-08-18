using FluentValidation;
using FootballManager.Application.Features.Members.Commands.Delete;

namespace FootballManager.Application.Features.Members.Validators
{
    public class DeleteMemberValidator : AbstractValidator<DeleteMemberCommand>
    {
        public DeleteMemberValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                              .GreaterThan(0);
        }
    }
}
