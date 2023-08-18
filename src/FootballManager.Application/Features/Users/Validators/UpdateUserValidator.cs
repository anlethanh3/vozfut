using FluentValidation;
using FootballManager.Application.Features.Users.Commands.Update;

namespace FootballManager.Application.Features.Users.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} required.")
                                .MaximumLength(100);
        }
    }
}
