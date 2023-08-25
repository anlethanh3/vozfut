using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Application.Features.Votes.Commands.Update
{
    internal class UpdateVoteCommandHandler : IRequestHandler<UpdateVoteCommand, Result<bool>>
    {
        private readonly IAsyncRepository<Vote> _voteRepository;

        public UpdateVoteCommandHandler(IAsyncRepository<Vote> voteRepository)
        {
            _voteRepository = voteRepository;
        }

        public async Task<Result<bool>> Handle(UpdateVoteCommand request, CancellationToken cancellationToken)
        {
            var vote = await _voteRepository.Entities.Where(x => x.Id == request.Id && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new DomainException("Vote not found");

            if (vote.Status.ToLower().Equals(VoteStatusEnum.Completed.Name.ToLower()))
            {
                throw new DomainException("The vote cannot be deleted because it is completed.");
            }

            vote.Name = request.Name;
            vote.Description = request.Description;
            vote.ModifiedDate = request.RequestedAt;
            vote.ModifiedBy = request.RequestedBy;

            var updated = await _voteRepository.UpdateAsync(vote);

            return updated != null ? Result<bool>.Success(true) : Result<bool>.Failure(false);
        }
    }
}
