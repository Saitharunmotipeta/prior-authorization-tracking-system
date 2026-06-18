using Microsoft.AspNetCore.Http;

namespace PriorAuthorization.Shared.Exceptions;

public abstract class AppException : Exception
{
    public int StatusCode { get; }

    protected AppException(
        int statusCode,
        string message)
        : base(message)
    {
        StatusCode = statusCode;
    }
}

public sealed class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(StatusCodes.Status404NotFound, message)
    {
    }
}

public sealed class ValidationException : AppException
{
    public ValidationException(string message)
        : base(StatusCodes.Status400BadRequest, message)
    {
    }
}

public sealed class ConflictException : AppException
{
    public ConflictException(string message)
        : base(StatusCodes.Status409Conflict, message)
    {
    }
}

public sealed class UnauthorizedAppException : AppException
{
    public UnauthorizedAppException(string message)
        : base(StatusCodes.Status401Unauthorized, message)
    {
    }
}