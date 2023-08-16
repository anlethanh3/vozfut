using FluentValidation;
using FootballManager.Application.Features.Matches.Commands.UpdateStatus;
using FootballManager.Domain.Enums;

namespace FootballManager.Application.Features.Matches.Validators
{
    public class UpdateStatusMatchValidator : AbstractValidator<UpdateStatusMatchCommand>
    {
        public UpdateStatusMatchValidator()
        {
            var status = string.Join(',', MatchStatusEnum.Gets.Select(x => x.Name).ToList());
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("{PropertyName} is required.")
                              .Must(x => x > 0);
            RuleFor(x => x.Status).NotEmpty().WithMessage("{PropertyName} is required.")
                                  .Must(x => MatchStatusEnum.Gets.Select(x => x.Name).Contains(x))
                                  .WithMessage($"Status must have in '{status}'");
        }
    }
}
