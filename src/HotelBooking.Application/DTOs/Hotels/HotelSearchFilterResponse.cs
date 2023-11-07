using HotelBooking.Application.Helpers;

namespace HotelBooking.Application.DTOs.Hotels;
public class HotelSearchFilterResponse
{
    public IList<object> SortFilters { get; set; } = Array.Empty<string>();

    public IList<object> MatrixFilters { get; set; } = Array.Empty<string>();
    public PaginatedResponse<HotelResponse> Data { get; set; } = default!;
}
