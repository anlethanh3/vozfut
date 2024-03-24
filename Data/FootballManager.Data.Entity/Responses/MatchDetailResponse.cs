namespace FootballManager.Data.Entity.Responses;

public class MatchDetailResponse
{
    public int Id { get; set; } = 0;
    public int MatchId { get; set; }
    public string MatchName { get; set; } = string.Empty;
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int MemberElo { get; set; }
    public bool IsPaid { get; set; }
    public bool IsSkip { get; set; }
    public bool IsWinner { get; set; }
    public int Goal { get; set; }
    public int Assist { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool IsDeleted { get; set; } = false;

}
