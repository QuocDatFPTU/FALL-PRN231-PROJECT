using HotelBooking.Application.DTOs.Areas;
using HotelBooking.Application.DTOs.Cities;
using HotelBooking.Application.Helpers;

namespace HotelBooking.Application.Interfaces.Services;
public interface ICityService
{
    Task<PaginatedList<CityResponse>> GetTopDestinationsAsync(int limit);
    Task<PaginatedList<AreaRecommendationResponse>> GetPlaceRecommendationsAsync(int cityId, int limit);
    Task<PaginatedList<CityResponse>> GetUnifiedSuggestResultAsync(string searchText, int limit);
}
