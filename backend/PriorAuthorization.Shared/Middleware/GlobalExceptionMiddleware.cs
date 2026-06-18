using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PriorAuthorization.Shared.Exceptions;
using PriorAuthorization.Shared.Common;

namespace PriorAuthorization.Shared.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                context,
                ex);
        }
    }

    private static Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        int statusCode =
            StatusCodes.Status500InternalServerError;

        string message =
            "An unexpected error occurred.";

        if (exception is AppException appException)
        {
            statusCode = appException.StatusCode;
            message = appException.Message;
        }

        var response =
            ApiResponse<object>
                .FailureResponse(message);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType =
            "application/json";

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}