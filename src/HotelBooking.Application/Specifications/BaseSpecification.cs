using HotelBooking.Application.Helpers;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HotelBooking.Application.Specifications;
public class BaseSpecification<T> : ISpecifications<T>
{
    public Expression<Func<T, bool>>? Criteria { get; set; }
    public BaseSpecification(Expression<Func<T, bool>>? Criteria)
    {
        this.Criteria = Criteria;
    }

    public List<Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>>> Includes { get; } = new();

    public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; private set; }

    protected void AddInclude(Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    public void AddOrderBy(Expression<Func<T, object>> OrderByexpression, Expression<Func<T, object>>? ThenOrderByexpression = null)
    {
        OrderBy = a => a.OrderBy(OrderByexpression);
        if (ThenOrderByexpression != null)
        {
            OrderBy = a => a.OrderBy(OrderByexpression).ThenBy(ThenOrderByexpression);
        }
    }
    public void AddOrderByDecending(Expression<Func<T, object>> OrderByDecending, Expression<Func<T, object>>? ThenOrderByDecending = null)
    {
        OrderBy = a => a.OrderByDescending(OrderByDecending);
        if (ThenOrderByDecending != null)
        {
            OrderBy = a => a.OrderByDescending(OrderByDecending).ThenBy(ThenOrderByDecending);
        }
    }

    public void AddFilter(Expression<Func<T, bool>> expression)
    {
        Criteria = Criteria.AndAlso(expression);
    }

}