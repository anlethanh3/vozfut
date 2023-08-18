namespace Application.Contracts.Entities
{
    public interface IModificationAuditObject
    {
        string ModifiedBy { get; }
        DateTime? ModifiedDate { get; }
    }
}
