using Application.Contracts.Entities;
using FootballManager.Domain.Entities;

namespace Domain.Entities.Audit
{
    public abstract class CreateAuditable<TId> : BaseEntity<TId>, ICreationAuditObject
    {
        public virtual string CreatedBy { get; set; }

        public virtual DateTime CreatedDate { get; set; }
    }
}
