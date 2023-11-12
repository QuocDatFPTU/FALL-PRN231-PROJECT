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
        return await _unitOfWork.Repository<City>()
                .FindAsync<CityResponse>(
                    configuration: _mapper.ConfigurationProvider,
                    pageIndex: 1,
                    pageSize: limit,
                    orderBy: _ => _.OrderByDescending(_ => _.NoOfHotels));
    }

    public async Task<PaginatedList<RecommendationAreaResponse>> GetPlaceRecommendationsAsync(int cityId, int limit)
    {
        return await _unitOfWork.Repository<Area>()
                .FindAsync<RecommendationAreaResponse>(
                    configuration: _mapper.ConfigurationProvider,
                    pageIndex: 1,
                    pageSize: limit,
                    expression: _ => _.CityId == cityId && _.NoOfHotels > 0,
                    orderBy: _ => _.OrderByDescending(_ => _.NoOfHotels));
    }

    public async Task<PaginatedList<CityResponse>> GetUnifiedSuggestResultAsync(string searchText, int limit)
    {
        return await _unitOfWork.Repository<City>()
                .FindAsync<CityResponse>(
                    configuration: _mapper.ConfigurationProvider,
                    pageIndex: 1,
                    pageSize: limit,
                    expression: _ => EF.Functions.Like(_.Name, $"%{searchText}%"));
    }
}
