using HotelBooking.Application.Common.Exceptions;
using HotelBooking.Application.DTOs.Hotels;
using HotelBooking.Application.Enums.Filters;
using HotelBooking.Application.Enums.Sorting;
using HotelBooking.Application.Helpers;
using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelBooking.Application.Specifications;
public class HotelSearchWithSpecification : BaseSpecification<Hotel>
{
    public HotelSearchWithSpecification(HotelSearchRequest request) : base
        (_ => _.Address.CityId == request.CityId)
    {
        switch (request.Sorting.SortOrder)
        {
            case SortOrder.Asc:
                switch (request.Sorting.SortField)
                {
                    case SortField.Ranking:
                        AddOrderBy(_ => _.ReviewCount, _ => _.Id);
                        break;
                    case SortField.Price:
                        AddOrderBy(_ => _.RoomTypes.Min(rt => rt.Price), _ => _.Id);
                        break;
                    case SortField.ReviewScore:
                        AddOrderBy(_ => _.ReviewRating, _ => _.Id);
                        break;
                }
                break;
            case SortOrder.Desc:

                switch (request.Sorting.SortField)
                {
                    case SortField.Ranking:
                        AddOrderByDecending(_ => _.ReviewCount, _ => _.Id);
                        break;
                    case SortField.Price:
                        AddOrderByDecending(_ => _.RoomTypes.Min(rt => rt.Price), _ => _.Id);
                        break;
                    case SortField.ReviewScore:
                        AddOrderByDecending(_ => _.ReviewRating, _ => _.Id);
                        break;
                }
                break;
        }

        var filterRequest = request.FilterRequest;

        if (filterRequest != null)
        {
            var idsFilters = filterRequest.IdsFilters;
            var rangeFilters = filterRequest.RangeFilters;
            var textFilters = filterRequest.TextFilters;

            if (!idsFilters.IsNullOrEmpty())
            {
                idsFilters = idsFilters!.DistinctBy(_ => _.FilterKey).ToList();
                foreach (var item in idsFilters!)
                {
                    switch (item.FilterKey)
                    {
                        case FilterKey.HotelAreaId:
                            AddFilter(_ => item.Ids.Contains(_.Address.AreaId));
                            break;
                        case FilterKey.AccommodationType:
                            AddFilter(_ => item.Ids.Contains(_.CategoryId));
                            break;
                        default:
                            throw new BadRequestException($"`{item.FilterKey}` not supporter for idsFilters");
                    }
                }
            }

            if (!rangeFilters.IsNullOrEmpty())
            {
                rangeFilters = rangeFilters!.DistinctBy(_ => _.FilterKey).ToList();
                foreach (var item in rangeFilters!)
                {
                    Expression<Func<Hotel, bool>> expression = _ => false;
                    switch (item.FilterKey)
                    {
                        case FilterKey.Price:

                            foreach (var range in item.Ranges)
                            {
                                expression = expression.OrElse(_ => _.RoomTypes.Any(_ => _.Price >= range.From && _.Price <= range.To));
                            }

                            AddFilter(expression);
                            break;
                        case FilterKey.StarRating:

                            foreach (var range in item.Ranges)
                            {
                                expression = expression.OrElse(_ => _.ReviewRating >= range.From && _.ReviewRating < range.To);
                            }

                            AddFilter(expression);
                            break;
                        default:
                            throw new BadRequestException($"`{item.FilterKey}` not supporter for rangeFilters");
                    }
                }
            }

            if (!textFilters.IsNullOrEmpty())
            {
                textFilters = textFilters!.DistinctBy(_ => _.FilterKey).ToList();
                foreach (var item in textFilters!)
                {
                    switch (item.FilterKey)
                    {
                        case FilterKey.Name:
                            AddFilter(_ => EF.Functions.Like(_.Name, $"%{item.Text}%"));
                            break;
                        default:
                            throw new BadRequestException($"`{item.FilterKey}` not supporter for textFilters");
                    }
                }
            }

        }

    }
}
