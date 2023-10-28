﻿namespace HotelBooking.Application.Common.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("You do not have access to this system") { }

    public ForbiddenAccessException(string message) : base(message) { }
}
