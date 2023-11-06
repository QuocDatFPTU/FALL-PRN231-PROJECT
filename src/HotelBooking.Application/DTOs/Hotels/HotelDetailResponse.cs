using HotelBooking.Application.Common.Mappings;
using HotelBooking.Application.DTOs.Addresses;
using HotelBooking.Application.DTOs.RoomTypes;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Hotels;
public class HotelDetailResponse : BaseAuditableEntity, IMapFrom<Hotel>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Notes { get; set; }
    public string? WebsiteAddress { get; set; }
    public int ReviewCount { get; set; }
    public int ReviewRating { get; set; }
    public int CategoryId { get; set; }

    public AddressResponse Address { get; set; } = default!;
    public CategoryResponse Category { get; set; } = default!;
    public ICollection<HotelImageResponse> HotelImages { get; set; } = new HashSet<HotelImageResponse>();
    public ICollection<HotelGroupResponse> HotelGroups { get; set; } = new HashSet<HotelGroupResponse>();
    public ICollection<HotelFacilityHighlightResponse> HotelFacilityHighlights { get; set; } = new HashSet<HotelFacilityHighlightResponse>();
    public ICollection<RoomTypeResponse> MasterRooms { get; set; } = new HashSet<RoomTypeResponse>();
    public ICollection<RoomTypeSoldOutResponse> SoldOutRooms { get; set; } = new HashSet<RoomTypeSoldOutResponse>();
}
