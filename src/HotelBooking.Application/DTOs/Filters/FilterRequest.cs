namespace HotelBooking.Application.DTOs.Filters;
public class FilterRequest
{
    public IList<IdsFilterRequest>? IdsFilters { get; set; }
    public IList<RangeFilterRequest>? RangeFilters { get; set; }
    public IList<TextFilterRequest>? TextFilters { get; set; }

}
