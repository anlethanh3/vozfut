using FluentValidation;
using FootballManager.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using FootballManager.Infrastructure.Extensions;

namespace FootballManager.Application.Behaviours
{
    public class ValidatorBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<ValidatorBehaviour<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidatorBehaviour(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidatorBehaviour<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var typeName = request.GetGenericTypeName();

            _logger.LogInformation("Validating command {CommandType}", typeName);

            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                _logger.LogError("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

                throw new DomainException(string.Join(",", failures.Select(x => x.ErrorMessage)));
            }

            return await next();
        }
    }
}
