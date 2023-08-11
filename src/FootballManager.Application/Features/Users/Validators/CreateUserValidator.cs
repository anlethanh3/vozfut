using FluentValidation;
using FootballManager.Application.Features.Users.Commands.Create;

namespace FootballManager.Application.Features.Users.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("{PropertyName} required")
                                    .MaximumLength(20);
            RuleFor(x => x.Password).NotEmpty().WithMessage("{PropertyName} required")
                                    .MaximumLength(20);
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} required")
                                .MaximumLength(40);
            RuleFor(x => x.Email).MaximumLength(50);
        }
    }
}
