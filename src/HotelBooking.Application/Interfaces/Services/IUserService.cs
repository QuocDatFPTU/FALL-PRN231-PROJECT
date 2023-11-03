using HotelBooking.Application.DTOs.Users;

namespace HotelBooking.Application.Interfaces.Services;
public interface IUserService
{
    Task<UserResponse> FindUserAsync(int id);
}
