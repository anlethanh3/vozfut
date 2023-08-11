using Domain.Entities.Audit;

namespace FootballManager.Domain.Entities;

public class User : FullAuditable<int>
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public int? MemberId { get; set; }
    public bool IsAdmin { get; set; }
}
