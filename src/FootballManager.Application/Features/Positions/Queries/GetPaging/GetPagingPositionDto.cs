namespace FootballManager.Application.Features.Positions.Queries.GetPaging
{
    public class GetPagingPositionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
