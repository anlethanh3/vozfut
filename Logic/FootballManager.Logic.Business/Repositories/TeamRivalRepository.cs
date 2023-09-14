using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Requests;
using FootballManager.Logic.Business.Interfaces;
using Newtonsoft.Json;

namespace FootballManager.Logic.Business.Repositories;
public class TeamRivalRepository : ITeamRivalRepository
{
    private readonly IMatchRepository matchRepository;
    private readonly IMatchDetailRepository matchDetailRepository;
    private readonly IMemberRepository memberRepository;
    public TeamRivalRepository(
        IMatchRepository matchRepository,
        IMatchDetailRepository matchDetailRepository,
        IMemberRepository memberRepository)
    {
        this.matchRepository = matchRepository;
        this.matchDetailRepository = matchDetailRepository;
        this.memberRepository = memberRepository;
    }

    public async Task<IEnumerable<TeamRival>> CreateAsync(int id)
    {
        var match = await matchRepository.GetAsync(id);
        if (match is null)
        {
            throw new Exception("ERR_MATCH_NOT_EXIST");
        }
        if (match.HasTeamRival)
        {
            throw new Exception("ERR_TEAM_RIVALS_EXISTED");
        }
        var details = await matchDetailRepository.GetAllAsync(id);
        var numTeams = match.TeamCount;
        var readyPlayers = details.Where(x => x.IsPaid && !x.IsSkip);
        var elos = new int[numTeams];
        if (readyPlayers.Count() < numTeams * match.TeamSize)
        {
            throw new Exception("ERR_TEAM_SIZE");
        }
        var players = readyPlayers
            .Select(x => new Member
            {
                Id = x.MemberId,
                Elo = x.MemberElo,
                Name = x.MemberName,
            });
        var orders = players.OrderByDescending(item => item.Elo).ToList();
        var teams = new List<Member>[match.TeamCount];
        var rng = new Random();
        // random members
        while (orders.Count() > 0)
        {
            int teamIndex = 0;
            for (var index = 0; index < elos.Count(); index++)
            {
                if (teams[index] is null)
                {
                    teams[index] = new();
                }
                if (elos[teamIndex] > elos[index])
                {
                    teamIndex = index;
                }
                else if (teams[teamIndex].Count() > teams[index].Count())
                {
                    teamIndex = index;
                }
            }
            var maxElos = orders.Where(x => x.Elo == orders[0].Elo).ToList();
            var randomIndex = rng.Next(-1, maxElos.Count() - 1) + 1;
            var player = maxElos[randomIndex];
            teams[teamIndex].Add(player);
            elos[teamIndex] += player.Elo;
            orders.Remove(player);
        }
        var result = teams.Select(x => new TeamRival
        {
            Players = x,
            EloSum = x.Sum(m => m.Elo),
        });
        var _ = await SaveAsync(id, result);
        return result;
    }
    public async Task<IEnumerable<TeamRival>> GetAsync(int id)
    {
        var match = await matchRepository.GetAsync(id);
        if (match is null)
        {
            throw new Exception("ERR_MATCH_NOT_EXIST");
        }
        if (!match.HasTeamRival)
        {
            throw new Exception("ERR_MATCH_NO_TEAM_RIVAL");
        }
        var result = JsonConvert.DeserializeObject<TeamRival[]>(match.TeamRivals);
        return result;
    }
    public async Task<bool> SaveAsync(int matchId, IEnumerable<TeamRival> teams)
    {
        var json = JsonConvert.SerializeObject(teams);
        var match = await matchRepository.GetAsync(matchId);
        if (match is null)
        {
            return false;
        }
        match.HasTeamRival = true;
        match.TeamRivals = json;
        var _ = await matchRepository.UpdateAsync(match);
        return true;
    }
    public async Task<bool> ExchangeMemberAsync(ExchangeMemberRequest model)
    {
        var match = await matchRepository.GetAsync(model.MatchId);
        if (match is null)
        {
            throw new Exception("ERR_MATCH_NOT_EXIST");
        }
        if (!match.HasTeamRival)
        {
            throw new Exception("ERR_MATCH_NO_TEAM_RIVAL");
        }
        var result = JsonConvert.DeserializeObject<TeamRival[]>(match.TeamRivals);
        var memberIn = await memberRepository.GetAsync(model.MemberInId);
        var memberOut = await memberRepository.GetAsync(model.MemberOutId);
        if (memberIn is null)
        {
            throw new Exception("ERR_MATCH_DETAIL_MEMBER_IN_NOT_EXIST");
        }
        if (memberOut is null)
        {
            throw new Exception("ERR_MATCH_DETAIL_MEMBER_OUT_NOT_EXIST");
        }
        foreach (var item in result)
        {
            var player = item.Players.FirstOrDefault(x => x.Id == memberOut.Id);
            if (player is not null)
            {
                item.EloSum += memberIn.Elo - player.Elo;
                item.Players.Remove(player);
                item.Players.Add(memberIn);
                break;
            }
        }
        var _ = await SaveAsync(model.MatchId, result);
        return true;
    }
}