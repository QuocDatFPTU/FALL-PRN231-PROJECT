using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelBooking.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext context;
    private readonly DbSet<T> dbSet;

    public GenericRepository(ApplicationDbContext _context)
    {
        context = _context;
        dbSet = context.Set<T>();
    }

    public virtual async Task CreateAsync(T entity)
    {
        await dbSet.AddAsync(entity);

    }

    public virtual async Task CreateRangeAsync(IEnumerable<T> entities)
    {
        await dbSet.AddRangeAsync(entities);
    }
    public virtual async Task<T?> FindByIdAsync(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public async Task<T?> FindByAsync(
       Expression<Func<T, bool>> expression,
       Func<IQueryable<T>, IQueryable<T>>? includeFunc = null)
    {
        IQueryable<T> query = dbSet;

        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        return await query.FirstOrDefaultAsync(expression);
    }

    public async Task<IList<T>> FindAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null)
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public virtual Task DeleteAsync(T entity)
    {
        dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(T entity)
    {
        //dbSet.Attach(entity);
        dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByAsync(Expression<Func<T, bool>>? expression = null)
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }

        return await query.AnyAsync();
    }

    public async Task<TDTO?> FindByAsync<TDTO>(
        IConfigurationProvider configuration,
        Expression<Func<T, bool>> expression) where TDTO : class
    {
        IQueryable<T> query = dbSet;

        query = query.Where(expression);

        return await query.ProjectTo<TDTO>(configuration).FirstOrDefaultAsync();
    }

    public async Task<IList<TDTO>> FindAsync<TDTO>(
        IConfigurationProvider configuration,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where TDTO : class
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ProjectTo<TDTO>(configuration).ToListAsync();
    }

    public async Task<PaginatedList<TDTO>> FindAsync<TDTO>(
        IConfigurationProvider configuration,
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where TDTO : class
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ProjectTo<TDTO>(configuration).PaginatedListAsync(pageIndex, pageSize);
    }
}