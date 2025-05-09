using Microsoft.OpenApi.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.OpenApi;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.OpenApi
{
    /// <summary>
    /// Add security requirement
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class SecurityRequirementOperationTransformer : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            var hasAllowAnonymous = context.Description.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any();
            if (hasAllowAnonymous)
            {
                return Task.CompletedTask;
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
            return Task.CompletedTask;
        }
    }
}
