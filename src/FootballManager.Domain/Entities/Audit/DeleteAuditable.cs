using Application.Contracts.Entities;
using FootballManager.Domain.Entities;

namespace Domain.Entities.Audit
{
    public abstract class DeleteAuditable<TId> : BaseEntity<TId>, IDeletionAuditObject
    {
        public virtual bool IsDeleted { get; set; }

        public virtual DateTime? DeletedDate { get; set; }
    }
}
