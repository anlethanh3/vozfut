using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Commands.UpdateStatus
{
    public record UpdateStatusMatchCommand(int Id, string Status) : RequestAudit, IRequest<Result<bool>>;

    internal class UpdateStatusMatchCommandHandler : IRequestHandler<UpdateStatusMatchCommand, Result<bool>>
    {
        private readonly IAsyncRepository<Match> _matchRepository;

        public UpdateStatusMatchCommandHandler(IAsyncRepository<Match> matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<Result<bool>> Handle(UpdateStatusMatchCommand request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetAsync(request.Id) ?? throw new DomainException("Match not found");

            if (match.Status.ToLower().Equals(MatchStatusEnum.Completed.Name.ToLower()))
            {
                throw new DomainException("Cannot update, because match completed");
            }

            match.Status = request.Status;
            match.ModifiedBy = request.RequestedBy;
            match.ModifiedDate = request.RequestedAt;

            var updated = await _matchRepository.UpdateAsync(match);
            return updated != null ? Result<bool>.Success(true) : Result<bool>.Failure(false);
        }
    }
}
