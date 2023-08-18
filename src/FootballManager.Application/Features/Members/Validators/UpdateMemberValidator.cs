using FluentValidation;
using FootballManager.Application.Features.Members.Commands.Update;

namespace FootballManager.Application.Features.Members.Validators
{
    public class UpdateMemberValidator : AbstractValidator<UpdateMemberCommand>
    {
        public UpdateMemberValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                                       .GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required.")
                                .MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(200);
            RuleFor(x => x.Elo).NotNull().WithMessage("{PropertyName} is required.")
                               .GreaterThan(0).WithMessage("{PropertyName} greater than zezo");
        }
    }
}
