using HotelBooking.Application.Enums.Sorting;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.DTOs.Sorting;
public class SortRequest
{
    [EnumDataType(typeof(SortField))]
    public SortField SortField { get; set; }

    [EnumDataType(typeof(SortOrder))]
    public SortOrder SortOrder { get; set; }
}
