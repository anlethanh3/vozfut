using FluentValidation;
using FootballManager.Application.Features.Users.Commands.Authenticate;

namespace FootballManager.Application.Features.Users.Validators
{
    public class AuthenticateUserValidator : AbstractValidator<AuthenticateUserCommand>
    {
        public AuthenticateUserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("{PropertyName} required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("{PropertyName} required");
        }
    }
}
