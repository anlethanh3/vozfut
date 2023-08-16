using FluentValidation;
using FootballManager.Application.Features.Matches.Commands.Create;

namespace FootballManager.Application.Features.Matches.Validators
{
    public class CreateMatchValidator : AbstractValidator<CreateMatchCommand>
    {
        public CreateMatchValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required.")
                                .MaximumLength(100);

            RuleFor(x => x.Description).MaximumLength(200);

            RuleFor(x => x.TeamSize).NotNull().WithMessage("{PropertyName} is required.")
                                    .Must(x => x > 0);

            RuleFor(x => x.TeamCount).NotNull().WithMessage("{PropertyName} is required.")
                                     .Must(x => x > 0);

            RuleFor(x => x.FootballFieldSize).NotNull().WithMessage("{Property} is required.")
                        .Must(x => x > 0);

            RuleFor(x => x.FootballFieldAddress).NotEmpty().WithMessage("{Property} is required.");

            RuleFor(x => x.FootballFieldNumber).NotEmpty().NotNull().WithMessage("{Property} is required.")
                        .Must(x => x > 0);
            RuleFor(x => x.StartTime).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");

            RuleFor(x => x.EndTime).NotEmpty().NotNull().WithMessage("{PropertyName} is required.")
                                   .GreaterThan(x => x.StartTime).WithMessage("EndTime must be greathan StartTime");

            RuleFor(x => x.TotalAmount).NotNull().WithMessage("{Property} is required.");

            RuleFor(x => x.TotalHour).NotNull().WithMessage("{Property} is required.")
                        .Must(x => x > 0);
        }
    }
}
