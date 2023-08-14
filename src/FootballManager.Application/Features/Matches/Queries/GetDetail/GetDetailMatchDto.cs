namespace FootballManager.Application.Features.Matches.Queries.GetDetail
{
    public class GetDetailMatchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int TeamSize { get; set; }
        public int TeamCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
