using FluentValidation;
using FootballManager.Application.Features.Users.Commands.Delete;

namespace FootballManager.Application.Features.Users.Validators
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("{PropertyName} not null")
                            .Must(x => x > 0);
        }
    }
}
