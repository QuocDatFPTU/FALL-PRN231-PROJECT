﻿using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HotelBooking.Infrastructure.Services;
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";

}
