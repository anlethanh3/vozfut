using Application.Contracts.Entities;

namespace FootballManager.Domain.Entities.Audit
{
    public abstract class CreModifiedAuditable<TId> : BaseEntity<TId>, ICreationAuditObject, IModificationAuditObject
    {
        public virtual DateTime CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedBy { get; set; }

        public virtual DateTime? ModifiedDate { get; set; }
    }
}
