using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Application.Features.Votes.Commands.Delete
{
    public record DeleteVoteCommand(int Id) : RequestAudit, IRequest<Result<bool>>;

    internal class DeleteVoteCommandHandler : IRequestHandler<DeleteVoteCommand, Result<bool>>
    {
        private readonly IAsyncRepository<Vote> _voteRepository;
        private readonly IAsyncRepository<MemberVote> _memberVoteRepository;

        public DeleteVoteCommandHandler(IAsyncRepository<Vote> voteRepository,
            IAsyncRepository<MemberVote> memberVoteRepository)
        {
            _voteRepository = voteRepository;
            _memberVoteRepository = memberVoteRepository;
        }

        public async Task<Result<bool>> Handle(DeleteVoteCommand request, CancellationToken cancellationToken)
        {
            var vote = await _voteRepository.Entities.Where(x=>x.Id==request.Id&&!x.IsDeleted).FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new DomainException("Vote not found");

            if (vote.Status.ToLower().Equals(VoteStatusEnum.Completed.Name.ToLower()))
            {
                throw new DomainException("The vote cannot be deleted because it is completed.");
            }

            var checkExistMemberVote = _memberVoteRepository.Entities.Where(x => x.VoteId == request.Id);
            if (checkExistMemberVote.Any())
            {
                throw new DomainException("Cannot delete vote, because already exists member in vote");
            }

            vote.IsDeleted = true;
            vote.DeletedDate = request.RequestedAt;

            var deleted = await _voteRepository.UpdateAsync(vote);

            return deleted != null ? Result<bool>.Success(true) : Result<bool>.Failure(false);
        }
    }
}
