namespace FootballManager.Data.Entity.Entities;

public class Scoreboard
{
    public int MatchId { get; set; }
    public int MatchDetailId { get; set; }
    public int MemberId { get; set; }
    public int Goal { get; set; }
    public int Assist { get; set; }
    public bool IsWinner { get; set; }
}
public class Leaderboard
{
    public int MemberId { get; set; }
    public string MemberName { get; set; }
    public int Goal { get; set; }
    public int Assist { get; set; }
}

public class WinnerTeamSize
{
    public int MemberId { get; set; }
    public string MemberName { get; set; }
    public int Winner { get; set; }
    public int TeamSize { get; set; }
}