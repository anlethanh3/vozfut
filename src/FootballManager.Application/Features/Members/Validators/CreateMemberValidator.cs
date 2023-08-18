using FluentValidation;
using FootballManager.Application.Features.Members.Commands.Create;

namespace FootballManager.Application.Features.Members.Validators
{
    internal class CreateMemberValidator : AbstractValidator<CreateMemberCommand>
    {
        public CreateMemberValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required.")
                                .MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(200);
            RuleFor(x => x.Elo).NotNull().WithMessage("{PropertyName} is required.")
                               .GreaterThan(0).WithMessage("{PropertyName} greater than zezo");
        }
    }
}
