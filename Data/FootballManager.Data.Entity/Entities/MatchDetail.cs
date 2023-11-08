namespace FootballManager.Data.Entity.Entities;

public class MatchDetail
{
    public int Id { get; set; } = 0;
    public int MatchId { get; set; }
    public int MemberId { get; set; }
    public int Goal { get; set; }
    public int Assist { get; set; }
    public bool IsPaid { get; set; }
    public bool IsSkip { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool IsDeleted { get; set; } = false;

}
