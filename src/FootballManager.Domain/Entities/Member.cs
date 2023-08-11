using Domain.Entities.Audit;

namespace FootballManager.Domain.Entities
{
    public class Member : FullAuditable<int>
    {
        public string Name { get; set; }
        public int Elo { get; set; }
        public string Description { get; set; }
    }
}
