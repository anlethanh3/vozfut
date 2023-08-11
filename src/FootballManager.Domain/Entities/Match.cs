using Domain.Entities.Audit;

namespace FootballManager.Domain.Entities;

public class Match : FullAuditable<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int TeamSize { get; set; }
    public int TeamCount { get; set; }
}
