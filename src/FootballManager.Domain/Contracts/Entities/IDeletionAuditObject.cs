namespace Application.Contracts.Entities
{
    public interface IDeletionAuditObject
    {
        bool IsDeleted { get; }
        DateTime? DeletedDate { get; }
    }
}
