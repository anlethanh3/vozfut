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

        public CreateMatchDetailCommandHandler(IAsyncRepository<MatchDetail> matchDetailRepository,
            IAsyncRepository<Match> matchRepository, IAsyncRepository<Member> memberRepository)
        {
            _matchDetailRepository = matchDetailRepository;
            _matchRepository = matchRepository;
            _memberRepository = memberRepository;
        }

        public async Task<Result<int>> Handle(CreateMatchDetailCommand request, CancellationToken cancellationToken)
        {
            _ = await _matchRepository.GetAsync(request.MatchId) ?? throw new DomainException("Match not found");
            _ = await _memberRepository.GetAsync(request.MemberId) ?? throw new DomainException("Member not found");
            var checkExist = _matchDetailRepository.Entities.Where(x => x.MatchId == request.MatchId && x.MemberId == request.MemberId).ToList();
            if (checkExist.Any())
            {
                throw new DomainException("The member already exists in the match");
            }

            var matchdetail = new MatchDetail
            {
                MatchId = request.MatchId,
                MemberId = request.MemberId,
                BibColour = request.BibColour,
                CreatedBy = request.RequestedBy,
                CreatedDate = request.RequestedAt
            };

            var created = await _matchDetailRepository.CreateAsync(matchdetail);

            return created != null ? await Result<int>.SuccessAsync(1) : await Result<int>.FailureAsync();
        }
    }
}
