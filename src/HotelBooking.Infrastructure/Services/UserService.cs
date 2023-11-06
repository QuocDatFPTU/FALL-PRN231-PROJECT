using AutoMapper;
using HotelBooking.Application.Common.Exceptions;
using HotelBooking.Application.DTOs.Users;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Infrastructure.Services;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UserService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserResponse> FindUserAsync(int id)
    {
        return (await _unitOfWork.Repository<Guest>()
                  .FindByAsync<UserResponse>(_mapper.ConfigurationProvider, _ => _.Id == id))
               .OrElseThrow(() => new NotFoundException(nameof(Guest), id));
    }
}
