using System.Linq.Expressions;

namespace FootballManager.Domain.Contracts.Repositories
{
    public interface IAsyncRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Entities { get; }

        Task<IReadOnlyList<TEntity>> GetAllAsync();

        Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                        string includes = null,
                                        bool disableTracking = true);

        Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                        List<Expression<Func<TEntity, object>>> includes = null,
                                        bool disableTracking = true);

        Task<TEntity> GetAsync(int id);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<List<TEntity>> CreateMultipleAsync(List<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(int id);
    }
}
