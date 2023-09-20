﻿using Middlewares.ExceptionHandling.Exceptions;

namespace Authentication.Application.Common.Exceptions.BadRequest;
public class InvalidAccessTokenBadRequestException : BadRequestException
{
    public InvalidAccessTokenBadRequestException(string username) : base(
        $"User '{username}': Invalid access token")
    {
        
    }
    public InvalidAccessTokenBadRequestException() : base("Invalid access token") {}
}