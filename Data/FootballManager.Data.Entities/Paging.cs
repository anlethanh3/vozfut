namespace FootballManager.Data.Entities;

public class Paging<T>
{
    public T? Data { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPage { get; set; }
}
