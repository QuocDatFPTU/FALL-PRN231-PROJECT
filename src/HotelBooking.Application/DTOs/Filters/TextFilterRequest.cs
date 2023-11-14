namespace HotelBooking.Application.DTOs.Filters;
public class TextFilterRequest : FilterKeyRequest
{
    public string Text { get; set; } = default!;
}
