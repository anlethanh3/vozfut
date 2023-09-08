namespace FootballManager.Data.Entity.Entities;

public class User
{
    public int Id { get; set; } = 0;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? MemberId { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string AvatarUri { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;

}
