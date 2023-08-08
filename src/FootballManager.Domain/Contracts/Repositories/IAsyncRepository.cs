using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Domain.Contracts.Repositories
{
    public interface IAsyncRepository<TEntity, TKey, TContext> where TEntity : class
                                                                where TContext : DbContext
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

        Task<TEntity> GetAsync(TKey id);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TKey id);
    }
}
