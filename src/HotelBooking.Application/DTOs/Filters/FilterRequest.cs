namespace HotelBooking.Application.DTOs.Filters;
public class FilterRequest
{
    public IList<IdsFilterRequest> IdsFilters { get; set; } = Array.Empty<IdsFilterRequest>();
    public IList<RangeFilterRequest> RangeFilters { get; set; } = Array.Empty<RangeFilterRequest>();
    public IList<TextFilterRequest> TextFilters { get; set; } = Array.Empty<TextFilterRequest>();

}
