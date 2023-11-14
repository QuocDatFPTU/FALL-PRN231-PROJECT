namespace HotelBooking.Application.DTOs.Filters;
public class IdsFilterRequest : FilterKeyRequest
{
    public int[] Ids { get; set; } = default!;
}
