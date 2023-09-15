namespace FootballManager.Data.Entity.Entities;

public class TeamRival
{
    public List<Member> Players { get; set; }
    public int EloSum { get; set; }
}

public class TeamRivalSchedule
{
    public List<TeamRivalMatch> HomeMatches { get; set; }
    public List<TeamRivalMatch> AwayMatches { get; set; }
    public List<string> Colors { get; set; }
}

public class TeamRivalMatch
{
    public int Home { get; set; }
    public int Away { get; set; }
}