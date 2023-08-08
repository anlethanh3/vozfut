using Application.Contracts.Entities;
using FootballManager.Domain.Entities;

namespace Domain.Entities.Audit
{
    public abstract class FullAuditable<TId> : BaseEntity<TId>, IFullAuditedObject
    {
        public virtual string CreatedBy { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual DateTime? ModifiedDate { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual DateTime? DeletedDate { get; set; }
    }
}
