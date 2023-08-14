using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Commands.Update
{
    internal class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand, Result<bool>>
    {
        private readonly IAsyncRepository<Match> _matchRepository;

        public UpdateMatchCommandHandler(IAsyncRepository<Match> matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<Result<bool>> Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetAsync(request.Id) ?? throw new DomainException("Match not found");

            match.Name = request.Name;
            match.Description = request.Description;
            match.TeamSize = request.TeamSize;
            match.TeamCount = request.TeamCount;
            match.ModifiedBy = request.RequestedBy;
            match.ModifiedDate = request.RequestedAt;

            var matchUpdated = await _matchRepository.UpdateAsync(match);
            if (matchUpdated != null)
            {
                return await Result<bool>.SuccessAsync(true);
            }
            else
            {
                return await Result<bool>.FailureAsync(false);
            }
        }
    }
}
