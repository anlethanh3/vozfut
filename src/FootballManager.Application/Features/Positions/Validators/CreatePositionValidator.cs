using FluentValidation;
using FootballManager.Application.Features.Positions.Commands.Create;

namespace FootballManager.Application.Features.Positions.Validators
{
    public class CreatePositionValidator : AbstractValidator<CreatePositionCommand>
    {
        public CreatePositionValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required.")
                              .MaximumLength(200);
            RuleFor(x => x.Code).NotEmpty().WithMessage("{PropertyName} is required.")
                              .MaximumLength(20);
            RuleFor(x => x.Description).MaximumLength(200);
        }
    }
}
