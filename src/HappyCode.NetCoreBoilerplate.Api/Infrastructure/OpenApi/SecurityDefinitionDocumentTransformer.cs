using Microsoft.OpenApi;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.OpenApi;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.OpenApi
{
    /// <summary>
    /// Define security schema
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class SecurityDefinitionDocumentTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
            document.Components.SecuritySchemes.Add("ApiKey", new OpenApiSecurityScheme
            {
                Description = "ApiKey needed to access the endpoints (eg: `Authorization: ApiKey xxx-xxx`)",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
            });
            return Task.CompletedTask;
        }
    }
}
