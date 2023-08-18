using FootballManager.Domain.ResultModels;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        ///  Paginated list from an <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The data type of an item in the list</typeparam>
        /// <param name="source">The IQueryable data source</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per page</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>A PaginatedResult <typeparamref name="T"/> that contains the items of the current page and pagination information</returns>
        public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken) where T : class
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            var count = await source.CountAsync(cancellationToken: cancellationToken);
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return PaginatedResult<T>.Create(items, count, pageNumber, pageSize);
        }
    }
}
