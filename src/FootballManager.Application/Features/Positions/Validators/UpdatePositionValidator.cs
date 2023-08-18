using FluentValidation;
using FootballManager.Application.Features.Positions.Commands.Update;

namespace FootballManager.Application.Features.Positions.Validators
{
    public class UpdatePositionValidator : AbstractValidator<UpdatePositionCommand>
    {
        public UpdatePositionValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                              .GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required.")
                              .MaximumLength(200);
            RuleFor(x => x.Code).NotEmpty().WithMessage("{PropertyName} is required.")
                              .MaximumLength(20);
            RuleFor(x => x.Description).MaximumLength(200);
        }
    }
}
