using HotelBooking.Application.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Application.Common.Mappings;
public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, int pageIndex, int pageSize) where TDestination : class
      => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageIndex, pageSize);
}
