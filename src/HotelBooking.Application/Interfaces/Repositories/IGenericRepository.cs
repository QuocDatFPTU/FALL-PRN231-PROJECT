using HotelBooking.Application.Helpers;
using System.Linq.Expressions;

namespace HotelBooking.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<bool> ExistsByAsync(Expression<Func<T, bool>>? expression = null);

        Task<T?> FindByIdAsync(int id);
        Task<T?> FindByAsync(
            Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);

        Task<IList<T>> FindAsync(
            Expression<Func<T, bool>>? expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);

        Task<TDTO?> FindByAsync<TDTO>(
            AutoMapper.IConfigurationProvider configuration,
            Expression<Func<T, bool>> expression) where TDTO : class;

        Task<IList<TDTO>> FindAsync<TDTO>(
            AutoMapper.IConfigurationProvider configuration,
            Expression<Func<T, bool>>? expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where TDTO : class;

        Task<PaginatedList<TDTO>> FindAsync<TDTO>(
            AutoMapper.IConfigurationProvider configuration,
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where TDTO : class;

        Task UpdateAsync(T entity);
        Task CreateAsync(T entity);
        Task CreateRangeAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
    }
}
