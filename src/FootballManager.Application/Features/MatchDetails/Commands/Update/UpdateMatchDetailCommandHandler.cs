using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MatchDetails.Commands.Update
{
    internal class UpdateMatchDetailCommandHandler : IRequestHandler<UpdateMatchDetailCommand, Result<bool>>
    {
        private readonly IAsyncRepository<MatchDetail> _matchDetailRepository;
        private readonly IAsyncRepository<Match> _matchRepository;
        private readonly IAsyncRepository<Member> _memberRepository;

        public UpdateMatchDetailCommandHandler(IAsyncRepository<MatchDetail> matchDetailRepository,
            IAsyncRepository<Match> matchRepository, IAsyncRepository<Member> memberRepository)
        {
            _matchDetailRepository = matchDetailRepository;
            _matchRepository = matchRepository;
            _memberRepository = memberRepository;
        }

        public async Task<Result<bool>> Handle(UpdateMatchDetailCommand request, CancellationToken cancellationToken)
        {
            var match = _matchRepository.Entities.Where(x => x.Id == request.MatchId && !x.IsDeleted).FirstOrDefault() ?? throw new DomainException("Match not found");
            if (match.Status.ToLower().Equals(MatchStatusEnum.Completed.Name.ToLower()))
            {
                throw new DomainException("Cannot update match detail, because match have completed");
            }
            _ = _memberRepository.Entities.Where(x => x.Id == request.MemberId && !x.IsDeleted).FirstOrDefault() ?? throw new DomainException("Member not found");
            var matchDetail = _matchDetailRepository.Entities.Where(x => x.Id == request.Id && !x.IsDeleted).FirstOrDefault() ?? throw new DomainException("MatchDetail not found");

            matchDetail.MatchId = request.MatchId;
            matchDetail.MemberId = request.MemberId;
            matchDetail.BibColour = request.BibColour;
            matchDetail.ModifiedBy = request.RequestedBy;
            matchDetail.ModifiedDate = request.RequestedAt;

            var updated = await _matchDetailRepository.UpdateAsync(matchDetail);

            return updated != null ? await Result<bool>.SuccessAsync(true) : Result<bool>.Failure(false);
        }
    }
}
