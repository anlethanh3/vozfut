using System.Security.Claims;
using FootballManager.Application.Extensions;
using FootballManager.Application.Features;
using MediatR;

namespace FootballManager.Application.Behaviours
{
    public class AuditBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ClaimsPrincipal _claimsPrincipal;

        public AuditBehaviour(ClaimsPrincipal claimsPrincipal)
        {
            _claimsPrincipal = claimsPrincipal;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Handling request
            if (request is RequestAudit)
            {
                var requestAudit = request as RequestAudit;
                requestAudit.RequestedAt = DateTime.UtcNow;
                requestAudit.RequestedBy = _claimsPrincipal.GetUsername() ?? "System";
                requestAudit.RequestUserId = _claimsPrincipal.GetUserId();
            }
            return await next();
        }
    }
}
