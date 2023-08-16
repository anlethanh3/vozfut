namespace FootballManager.Domain.Contracts.Entities
{
    public interface ICreModifiedAuditable
    {
        string CreatedBy { get; }
        DateTime CreatedDate { get; }
        string ModifiedBy { get; }
        DateTime? ModifiedDate { get; }
    }
}
