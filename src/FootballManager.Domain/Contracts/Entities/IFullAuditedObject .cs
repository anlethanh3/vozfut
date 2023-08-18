namespace Application.Contracts.Entities
{
    public interface IFullAuditedObject : ICreationAuditObject,
                                          IModificationAuditObject,
                                          IDeletionAuditObject
    {
    }
}
