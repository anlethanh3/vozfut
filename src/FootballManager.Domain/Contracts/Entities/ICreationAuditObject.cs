namespace Application.Contracts.Entities
{
    public interface ICreationAuditObject
    {
        string CreatedBy { get; }
        DateTime CreatedDate { get; }
    }
}
