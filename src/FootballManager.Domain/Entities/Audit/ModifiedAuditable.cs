using Application.Contracts.Entities;
using FootballManager.Domain.Entities;

namespace Domain.Entities.Audit
{
    public abstract class ModifiedAuditable<TId> : BaseEntity<TId>, IModificationAuditObject
    {
        public virtual string ModifiedBy { get; set; }

        public virtual DateTime? ModifiedDate { get; set; }
    }
}
