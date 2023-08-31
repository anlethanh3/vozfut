using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using FootballManager.Persistence.Context;
using MediatR;

namespace FootballManager.Application.Features.MatchDetails.Commands.Create
{
    internal class CreateMatchDetailCommandHandler : IRequestHandler<CreateMatchDetailCommand, Result<int>>
    {
        private readonly IAsyncRepository<MatchDetail> _matchDetailRepository;
        private readonly IAsyncRepository<Match> _matchRepository;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<MemberVote> _memberVoteRepository;
        private readonly IAsyncRepository<MatchScore> _matchScoreRepository;
        private readonly EfDbContext _efDbContext;

        public CreateMatchDetailCommandHandler(IAsyncRepository<MatchDetail> matchDetailRepository,
            IAsyncRepository<Match> matchRepository, IAsyncRepository<Member> memberRepository,
            IAsyncRepository<MemberVote> memberVoteRepository,
            IAsyncRepository<MatchScore> matchScoreRepository,
            EfDbContext efDbContext)
        {
            _matchDetailRepository = matchDetailRepository;
            _matchRepository = matchRepository;
            _memberRepository = memberRepository;
            _memberVoteRepository = memberVoteRepository;
            _matchScoreRepository = matchScoreRepository;
            _efDbContext = efDbContext;
        }

        public async Task<Result<int>> Handle(CreateMatchDetailCommand request, CancellationToken cancellationToken)
        {
            using var transaction = _efDbContext.Database.BeginTransaction();
            try
            {
                var matchDetail = new List<MatchDetail>();

                var match = _matchRepository.Entities.Where(x => x.Id == request.Teams.FirstOrDefault().MatchId && !x.IsDeleted).FirstOrDefault() ?? throw new DomainException("Match not found");

                foreach (var item in request.Teams)
                {
                    foreach (var member in item.MemberId)
                    {
                        _ = _memberRepository.Entities.Where(x => x.Id == member && !x.IsDeleted).FirstOrDefault() ?? throw new DomainException("Member not found");

                        //member không nằm trong vote thì không được tham gia match
                        //_ = _memberVoteRepository.Entities.Where(x => x.VoteId == match.VoteId && x.MemberId == member && x.IsJoin == true)
                        //                                  .FirstOrDefault() ?? throw new DomainException("Member is not exist in vote.");

                        var checkExist = _matchDetailRepository.Entities.Where(x => x.MatchId == match.Id && x.MemberId == member).ToList();
                        if (checkExist.Any())
                        {
                            throw new DomainException("The member already exists in the match");
                        }
                        matchDetail.Add(new MatchDetail
                        {
                            MatchId = request.Teams.First().MatchId,
                            MemberId = member,
                            BibColour = item.BibColour,
                            IsPaid = true,
                            IsSkip = false,
                            CreatedBy = request.RequestedBy,
                            CreatedDate = request.RequestedAt
                        });
                    }
                }

                // Tiến hành chia team random đối đầu theo vòng tròn
                var teams = request.Teams.Select(x => x.BibColour).ToList();
                var matches = GenerateSchedule(teams);
                var matchScores = new List<MatchScore>();
                foreach (var item in matches)
                {
                    matchScores.Add(new MatchScore
                    {
                        MatchId = match.Id,
                        Team1 = item.TeamA,
                        Team2 = item.TeamB,
                        CreatedBy = request.RequestedBy,
                        CreatedDate = request.RequestedAt
                    });
                }

                _ = await _matchScoreRepository.CreateMultipleAsync(matchScores);

                var created = await _matchDetailRepository.CreateMultipleAsync(matchDetail);
                transaction.Commit();
                return created != null ? await Result<int>.SuccessAsync(1) : await Result<int>.FailureAsync();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        private List<TeamDto> GenerateSchedule(List<string> teams)
        {
            if (teams.Count == 3)
            {
                return GenerateMatches(teams, 9);
            }
            else if (teams.Count == 4)
            {
                return GenerateMatches(teams, 12);
            }
            else
            {
                throw new ArgumentException("so luong doi khong hop le");
            }
        }

        private List<TeamDto> GenerateMatches(List<string> teams, int numberOfMatches)
        {
            var schedule = new List<TeamDto>();
            var shuffledTeams = new List<string>(teams);
            var random = new Random();

            for (var i = 0; i < numberOfMatches / teams.Count; i++)
            {
                shuffledTeams = shuffledTeams.OrderBy(x => random.Next()).ToList();

                for (var j = 0; j < teams.Count; j++)
                {
                    var nextTeamIndex = (j + 1) % shuffledTeams.Count;
                    schedule.Add(new TeamDto { TeamA = shuffledTeams[j], TeamB = shuffledTeams[nextTeamIndex] });
                    //if (!HasConsecutiveMatches(schedule, shuffledTeams[j]) && !HasConsecutiveMatches(schedule, shuffledTeams[nextTeamIndex]))
                    //{
                    //    schedule.Add(new Match { TeamA = shuffledTeams[j], TeamB = shuffledTeams[nextTeamIndex] });
                    //}
                }
            }

            return schedule;
        }

        private bool HasConsecutiveMatches(List<TeamDto> schedule, string team)
        {
            var count = 0;
            for (var i = schedule.Count - 1; i >= 0; i--)
            {
                if (schedule[i].TeamA == team || schedule[i].TeamB == team)
                {
                    count++;
                    if (count >= 2)
                    {
                        return true;
                    }
                }
                else
                {
                    break;
                }
            }
            return false;
        }
    }
}
