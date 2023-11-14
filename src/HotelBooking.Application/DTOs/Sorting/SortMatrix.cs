using HotelBooking.Application.Enums.Sorting;

namespace HotelBooking.Application.DTOs.Sorting;
public class SortMatrix
{
    public SortField FieldId { get; set; }
    public SortRequest Sorting { get; set; } = default!;
    public string Display { get; set; } = default!;
}
