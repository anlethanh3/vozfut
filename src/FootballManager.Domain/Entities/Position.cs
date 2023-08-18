using FootballManager.Domain.Entities.Audit;

namespace FootballManager.Domain.Entities
{
    public class Position : CreModifiedAuditable<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
