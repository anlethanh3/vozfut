namespace Application.Contracts.Entities
{
    /// <summary>
    /// Defines an entity with a single primary key with "Id" property.
    /// </summary>
    /// <typeparam name="TId">Type of the primary key of the entity</typeparam>
    public interface IEntity<TId>
    {
        TId Id { get; }
    }
}
