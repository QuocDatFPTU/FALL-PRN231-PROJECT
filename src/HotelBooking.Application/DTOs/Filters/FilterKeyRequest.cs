using HotelBooking.Application.Enums.Filters;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.DTOs.Filters;
public class FilterKeyRequest
{
    [EnumDataType(typeof(FilterKey))]
    public FilterKey FilterKey { get; set; }
}
