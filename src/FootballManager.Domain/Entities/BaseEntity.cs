using Application.Contracts.Entities;

namespace FootballManager.Domain.Entities
{
    public abstract class BaseEntity<TId> : IEntity<TId>
    {
        public virtual TId Id { get; set; }
    }
}
