using System.Linq.Expressions;
using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Persistence.Repositories
{
    public abstract class EfBaseRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : class
    {
        protected readonly EfDbContext GenericContext;

        public EfBaseRepository(EfDbContext genericContext)
        {
            GenericContext = genericContext;
        }

        public IQueryable<TEntity> Entities => GenericContext.Set<TEntity>();

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            GenericContext.Set<TEntity>().Add(entity);
            await GenericContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await GetAsync(id) ?? throw new Exception($"cannot find entity with id: {id}");

            GenericContext.Set<TEntity>().Remove(data);
            await GenericContext.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await GenericContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GenericContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
                                               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                               string includes = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = GenericContext.Set<TEntity>();

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(includes))
            {
                query = query.Include(includes);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                     List<Expression<Func<TEntity, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = GenericContext.Set<TEntity>();

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await GenericContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            GenericContext.Entry(entity).State = EntityState.Detached;
            GenericContext.Set<TEntity>().Update(entity);
            await GenericContext.SaveChangesAsync();

            return entity;
        }
    }
}
