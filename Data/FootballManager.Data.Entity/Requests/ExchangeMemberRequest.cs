namespace FootballManager.Data.Entity.Requests;

public class ExchangeMemberRequest
{
    public int MatchId { get; set; }
    public int MemberInId { get; set; }
    public int MemberOutId { get; set; }
}

public class MemberInOutRequest
{
    public int MatchId { get; set; }
    public int MemberId { get; set; }
    public int TeamId { get; set; }
    public bool IsIn { get; set; }
    public bool IsGK { get; set; }
}

public class WinnerUpdateRequest
{
    public int MatchId { get; set; }
    public int TeamId { get; set; }
    public int Number { get; set; }
}