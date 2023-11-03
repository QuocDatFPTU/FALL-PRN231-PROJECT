using HotelBooking.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HotelBooking.WebApi.Extensions;

public static class JwtBearerOptionsExtensions
{
    public static void HandleEvents(this JwtBearerOptions options)
    {
        options.Events = new JwtBearerEvents
        {
            OnForbidden = context =>
            {
                throw new ForbiddenAccessException();
            },

            OnChallenge = context =>
            {
                throw new UnauthorizedAccessException("You are not authorized to access this resource");
            },
        };
    }
}