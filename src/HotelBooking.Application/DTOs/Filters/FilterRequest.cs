namespace HotelBooking.Application.DTOs.Filters;
public class FilterRequest
{
    public IList<(string FilterKey, int[] Ids)> IdsFilters { get; set; } = default!;
    public IList<(string FilterKey, (object From, object To)[] Ranges)> RangeFilters { get; set; } = default!;
    public IList<(string FilterKey, string Text)> TextFilters { get; set; } = default!;

}
