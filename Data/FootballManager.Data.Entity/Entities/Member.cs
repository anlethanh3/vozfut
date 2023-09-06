namespace FootballManager.Data.Entity.Entities;

public class Member
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string RealName { get; set; } = string.Empty;
    public int Elo { get; set; } = 1;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool IsDeleted { get; set; } = false;
    public int Speed { get; set; }
    public int Stamina { get; set; }
    public int Finishing { get; set; }
    public int Passing { get; set; }
    public int Skill { get; set; }
}
