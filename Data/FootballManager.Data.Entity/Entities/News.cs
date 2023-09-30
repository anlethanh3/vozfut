namespace FootballManager.Data.Entity.Entities;

public class News
{
    public int Id { get; set; } = 0;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ImageUris { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool IsDeleted { get; set; } = false;

}
