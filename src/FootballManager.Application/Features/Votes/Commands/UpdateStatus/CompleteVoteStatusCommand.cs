using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Votes.Commands.UpdateStatus
{
    public record CompleteVoteStatusCommand(int Id) : RequestAudit, IRequest<Result<bool>>;

    internal class CompleteVoteStatusCommandHandler : IRequestHandler<CompleteVoteStatusCommand, Result<bool>>
    {
        private readonly IAsyncRepository<Vote> _voteRepository;
        private readonly IAsyncRepository<MemberVote> _memberVoteRepository;

        public CompleteVoteStatusCommandHandler(IAsyncRepository<Vote> voteRepository,
            IAsyncRepository<MemberVote> memberVoteRepository)
        {
            _voteRepository = voteRepository;
            _memberVoteRepository = memberVoteRepository;
        }

        public async Task<Result<bool>> Handle(CompleteVoteStatusCommand request, CancellationToken cancellationToken)
        {
            var vote = _voteRepository.Entities.Where(x => x.Id == request.Id && !x.IsDeleted).FirstOrDefault() ?? throw new DomainException("Vote not found");

            var checkExistMemberVote = _memberVoteRepository.Entities.Where(x => x.VoteId == request.Id);
            if (!checkExistMemberVote.Any())
            {
                throw new DomainException("Cannot completed vote, because vote empty");
            }

            vote.Status = VoteStatusEnum.Completed.Name;
            vote.ModifiedBy = request.RequestedBy;
            vote.ModifiedDate = request.RequestedAt;

            var updated = await _voteRepository.UpdateAsync(vote);

            return updated != null ? Result<bool>.Success(true) : Result<bool>.Failure(false);
        }
    }
}
