using Microsoft.OpenApi.Models;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters
{
    public class SecurityRequirementSwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAllowAnonymous = context.ApiDescription.CustomAttributes().OfType<IAllowAnonymous>().Any();
            if (hasAllowAnonymous)
            {
                return;
            }

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "ApiKey",
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey",
                        },
                    },
                    Array.Empty<string>()
                }
            });
        }
    }
}
