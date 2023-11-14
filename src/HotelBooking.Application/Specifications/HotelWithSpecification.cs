using HotelBooking.Application.DTOs.Hotels;
using HotelBooking.Application.Enums.Sorting;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Specifications;
public class HotelWithSpecification : BaseSpecification<Hotel>
{
    public HotelWithSpecification(HotelSearchRequest request) : base
        (_ => _.Address.CityId == request.CityId)
    {

        switch (request.Sorting.SortOrder)
        {
            case SortOrder.Asc:
                switch (request.Sorting.SortField)
                {
                    case SortField.Ranking:
                        AddOrderBy(_ => _.ReviewCount);
                        break;
                    case SortField.Price:
                        AddOrderBy(_ => _.RoomTypes.Min(rt => rt.Price));
                        break;
                    case SortField.ReviewScore:
                        AddOrderBy(_ => _.ReviewRating);
                        break;
                }
                break;
            case SortOrder.Desc:

                switch (request.Sorting.SortField)
                {
                    case SortField.Ranking:
                        AddOrderByDecending(_ => _.ReviewCount);
                        break;
                    case SortField.Price:
                        AddOrderByDecending(_ => _.RoomTypes.Min(rt => rt.Price));
                        break;
                    case SortField.ReviewScore:
                        AddOrderByDecending(_ => _.ReviewRating);
                        break;
                }
                break;
        }

    }
}
