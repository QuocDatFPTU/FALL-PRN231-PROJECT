using HotelBooking.Application.DTOs.Filters;
using HotelBooking.Application.DTOs.Sorting;
using HotelBooking.Application.Helpers;

namespace HotelBooking.Application.DTOs.Hotels;
public class HotelSearchFilterResponse
{
    public IList<SortMatrix> SortMatrix { get; set; } = Array.Empty<SortMatrix>();
    public IList<MatrixGroup> MatrixGroupFilters { get; set; } = Array.Empty<MatrixGroup>();
    public PaginatedResponse<HotelResponse> Data { get; set; } = default!;
}
