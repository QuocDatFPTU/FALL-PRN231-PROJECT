namespace HotelBooking.Application.DTOs.Filters;
public class RangeFilterRequest : FilterKeyRequest
{
    public Range[] Ranges { get; set; } = default!;
}

public class Range
{
    public double From { get; set; }
    public double To { get; set; }
}
