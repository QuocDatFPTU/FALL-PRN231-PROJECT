using HotelBooking.Application.Enums.Filters;

namespace HotelBooking.Application.DTOs.Filters;
public class MatrixGroup
{
    public FilterKey FilterKeyGroup { get; set; }
    public IList<MatrixItem> MatrixItems { get; set; } = default!;
}
