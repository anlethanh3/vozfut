namespace FootballManager.Data.Entity.Requests;

public class SearchPagingRequest
{
    public string Name { get; set; } = string.Empty;
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}