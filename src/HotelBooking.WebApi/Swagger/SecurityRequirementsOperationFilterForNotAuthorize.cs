using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HotelBooking.WebApi.Swagger;

public class SecurityRequirementsOperationFilterForNotAuthorize : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var requiredScopes = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<HttpPostAttribute>()
            .Select(attr => attr.Template)
            .Where(template => template.Contains("return"))
            .Distinct();

        if (requiredScopes.Any())
        {
            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            };
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [oAuthScheme] = new string[] { }
            });

        }
    }
}
