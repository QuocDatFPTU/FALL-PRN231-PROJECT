using AutoMapper;
using HotelBooking.Application.DTOs.Cities;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;

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
}
