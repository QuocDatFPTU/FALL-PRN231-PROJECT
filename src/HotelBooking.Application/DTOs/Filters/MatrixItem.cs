using HotelBooking.Application.Enums.Filters;

namespace HotelBooking.Application.DTOs.Filters;
public class MatrixItem
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int Count { get; set; }
    public FilterKey FilterKey { get; set; }
    public FilterRequestType FilterRequestType { get; set; }
}
