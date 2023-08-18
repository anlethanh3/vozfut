using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Commands.Delete
{
    public record DeleteMatchCommand(int Id) : RequestAudit, IRequest<Result<bool>>;

    internal class DeleteMatchCommandHandler : IRequestHandler<DeleteMatchCommand, Result<bool>>
    {
        private readonly IAsyncRepository<Match> _matchRepository;

        public DeleteMatchCommandHandler(IAsyncRepository<Match> matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<Result<bool>> Handle(DeleteMatchCommand request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetAsync(request.Id) ?? throw new DomainException("Match not found");
            if (match.IsDeleted)
            {
                throw new DomainException("Match not found");
            }

            if (match.Status.ToLower().Equals(MatchStatusEnum.Completed.Name.ToLower()))
            {
                throw new DomainException("cannot delete, because match completed");
            }

            match.IsDeleted = true;
            match.DeletedDate = request.RequestedAt;

            var deleted = await _matchRepository.UpdateAsync(match);
            return deleted != null
                           ? Result<bool>.Success(true)
                           : Result<bool>.Failure(false);
        }
    }
}
