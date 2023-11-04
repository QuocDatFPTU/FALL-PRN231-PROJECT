using AutoMapper;
using HotelBooking.Application.DTOs.Areas;
using HotelBooking.Application.DTOs.Cities;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Services;
public class CityService : ICityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CityService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<PaginatedList<CityResponse>> GetTopDestinationsAsync(int limit)
    {
        var cityIQ = await _unitOfWork.Repository<City>().FindToIQueryableAsync(orderBy: _ => _.OrderByDescending(_ => _.NoOfHotels));
        var paginatedCities = await _mapper.ProjectTo<CityResponse>(cityIQ).PaginatedListAsync(1, limit);
        return paginatedCities;
    }

    public async Task<PaginatedList<AreaRecommendationResponse>> GetPlaceRecommendationsAsync(int cityId, int limit)
    {
        var areaIQ = await _unitOfWork.Repository<Area>()
            .FindToIQueryableAsync(_ => _.CityId == cityId && _.NoOfHotels > 0, orderBy: _ => _.OrderByDescending(_ => _.NoOfHotels));
        var paginatedAreas = await _mapper.ProjectTo<AreaRecommendationResponse>(areaIQ).PaginatedListAsync(1, limit);
        return paginatedAreas;
    }

    public async Task<PaginatedList<CityResponse>> GetUnifiedSuggestResultAsync(string searchText, int limit)
    {
        var cityIQ = await _unitOfWork.Repository<City>().FindToIQueryableAsync(_ => EF.Functions.Like(_.Name, $"%{searchText}%"));
        var paginatedCities = await _mapper.ProjectTo<CityResponse>(cityIQ).PaginatedListAsync(1, limit);
        return paginatedCities;
    }
}
