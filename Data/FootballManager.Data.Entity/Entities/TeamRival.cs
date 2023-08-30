namespace FootballManager.Data.Entity.Entities;

public class TeamRival
{
    public List<Member> Players { get; set; }
    public int EloSum { get; set; }
}
