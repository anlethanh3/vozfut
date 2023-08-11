using Domain.Entities.Audit;

namespace FootballManager.Domain.Entities;

public class MatchDetail : FullAuditable<int>
{
    public int MatchId { get; set; }
    public int MemberId { get; set; }
    public bool IsPaid { get; set; }
    public bool IsSkip { get; set; }
}
