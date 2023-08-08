using Domain.Entities.Audit;

namespace FootballManager.Domain.Entities
{
    public class Member : FullAuditable<int>
    {
        public string Name { get; set; } = string.Empty;
        public int Elo { get; set; } = 1;
        public string Description { get; set; } = string.Empty;
    }
}
