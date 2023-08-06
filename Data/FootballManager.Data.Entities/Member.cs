namespace FootballManager.Data.Entities;

public class Member
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public int Elo { get; set; } = 1;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool IsDeleted { get; set; } = false;
    
}
