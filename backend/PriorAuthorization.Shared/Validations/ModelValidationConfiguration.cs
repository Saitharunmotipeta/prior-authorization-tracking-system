using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PriorAuthorization.Shared.Common;

namespace PriorAuthorization.Shared.Validations;

public static class ModelValidationConfiguration
{
    public static IServiceCollection
        AddModelValidationConfiguration(
            this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(
            options =>
            {
                options.InvalidModelStateResponseFactory =
                    context =>
                    {
                        var message =
                            context.ModelState
                                .FirstOrDefault(
                                    x => x.Value!.Errors.Count > 0)
                                .Value?
                                .Errors[0]
                                .ErrorMessage
                            ?? "Invalid request data";

                        return new BadRequestObjectResult(
                            ApiResponse<object>
                                .FailureResponse(message));
                    };
            });

        return services;
    }
}