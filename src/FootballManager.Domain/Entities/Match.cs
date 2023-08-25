using Domain.Entities.Audit;

namespace FootballManager.Domain.Entities;

public class Match : FullAuditable<int>
{
    public int? VoteId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public short TeamSize { get; set; }
    public short TeamCount { get; set; }
    public double TotalAmount { get; set; }
    public double TotalHour { get; set; }
    public short FootballFieldSize { get; set; }
    public string FootballFieldAddress { get; set; }
    public short FootballFieldNumber { get; set; }
    public string Description { get; set; }
    public DateTime MatchDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Refer: MatchStatusEnum
    /// </summary>
    public string Status { get; set; }
}
