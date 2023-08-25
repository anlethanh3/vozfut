using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Commands.Update
{
    internal class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand, Result<bool>>
    {
        private readonly IAsyncRepository<Match> _matchRepository;
        private readonly IAsyncRepository<Vote> _voteRepository;

        public UpdateMatchCommandHandler(IAsyncRepository<Match> matchRepository,
                IAsyncRepository<Vote> voteRepository)
        {
            _matchRepository = matchRepository;
            _voteRepository = voteRepository;
        }

        public async Task<Result<bool>> Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
        {
            var match = _matchRepository.Entities.Where(x => x.Id == request.Id && !x.IsDeleted).FirstOrDefault() ?? throw new DomainException("Match not found");

            if (match.Status.ToLower().Equals(MatchStatusEnum.Completed.Name.ToLower()))
            {
                throw new DomainException("Cannot update, because match completed");
            }
            match.Name = request.Name;
            match.Description = request.Description;
            match.TeamSize = request.TeamSize;
            match.TeamCount = request.TeamCount;
            match.TotalHour = request.TotalHour;
            match.TotalAmount = request.TotalAmount;
            match.FootballFieldSize = request.FootballFieldSize;
            match.FootballFieldAddress = request.FootballFieldAddress;
            match.FootballFieldNumber = request.FootballFieldNumber;
            match.StartTime = request.StartTime;
            match.EndTime = request.EndTime;
            match.ModifiedBy = request.RequestedBy;
            match.ModifiedDate = request.RequestedAt;
            if (request.VoteId.HasValue)
            {
                _ = _voteRepository.Entities.Where(x => x.Id == request.VoteId.Value && !x.IsDeleted) ?? throw new DomainException("Vote not found");
                match.VoteId = request.VoteId.Value;
            }

            var matchUpdated = await _matchRepository.UpdateAsync(match);
            return matchUpdated != null
                                ? Result<bool>.Success(true)
                                : Result<bool>.Failure(false);
        }
    }
}
