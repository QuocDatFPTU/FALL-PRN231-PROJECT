using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Hotels;
public class CategoryResponse : BaseEntity, IMapFrom<Category>
{
    public string Name { get; set; } = default!;
}
