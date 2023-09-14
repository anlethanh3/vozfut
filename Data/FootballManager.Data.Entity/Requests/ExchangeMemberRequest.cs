namespace FootballManager.Data.Entity.Requests;

public class ExchangeMemberRequest
{
    public int MatchId { get; set; }
    public int MemberInId { get; set; }
    public int MemberOutId { get; set; }
}