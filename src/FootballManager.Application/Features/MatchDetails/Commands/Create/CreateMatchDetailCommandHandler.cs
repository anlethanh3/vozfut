using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MatchDetails.Commands.Create
{
    internal class CreateMatchDetailCommandHandler : IRequestHandler<CreateMatchDetailCommand, Result<int>>
    {
        private readonly IAsyncRepository<MatchDetail> _matchDetailRepository;
        private readonly IAsyncRepository<Match> _matchRepository;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<MemberVote> _memberVoteRepository;

        public CreateMatchDetailCommandHandler(IAsyncRepository<MatchDetail> matchDetailRepository,
            IAsyncRepository<Match> matchRepository, IAsyncRepository<Member> memberRepository,
            IAsyncRepository<MemberVote> memberVoteRepository)
        {
            _matchDetailRepository = matchDetailRepository;
            _matchRepository = matchRepository;
            _memberRepository = memberRepository;
            _memberVoteRepository = memberVoteRepository;
        }

        public async Task<Result<int>> Handle(CreateMatchDetailCommand request, CancellationToken cancellationToken)
        {
            var match = _matchRepository.Entities.Where(x => x.Id == request.MatchId && !x.IsDeleted).FirstOrDefault() ?? throw new DomainException("Match not found");

            var matchDetail = new List<MatchDetail>();
            foreach (var item in request.MemberId)
            {
                _ = _memberRepository.Entities.Where(x => x.Id == item && !x.IsDeleted).FirstOrDefault() ?? throw new DomainException("Member not found");

                //member không nằm trong vote thì không được tham gia match
                _ = _memberVoteRepository.Entities.Where(x => x.VoteId == match.VoteId && x.MemberId == item && x.IsJoin == true)
                                                  .FirstOrDefault() ?? throw new DomainException("Member is not exist in vote.");

                var checkExist = _matchDetailRepository.Entities.Where(x => x.MatchId == request.MatchId && x.MemberId == item).ToList();
                if (checkExist.Any())
                {
                    throw new DomainException("The member already exists in the match");
                }
                matchDetail.Add(new MatchDetail
                {
                    MatchId = request.MatchId,
                    MemberId = item,
                    BibColour = request.BibColour,
                    IsPaid = true,
                    IsSkip = false,
                    CreatedBy = request.RequestedBy,
                    CreatedDate = request.RequestedAt
                });
            }
            var created = await _matchDetailRepository.CreateMultipleAsync(matchDetail);

            return created != null ? await Result<int>.SuccessAsync(1) : await Result<int>.FailureAsync();
        }
    }
}
