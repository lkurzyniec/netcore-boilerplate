using System.Diagnostics.CodeAnalysis;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.OpenApi
{
    [ExcludeFromCodeCoverage]
    public static class OpenApiRegistrations
    {
        public static void AddOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            string secretKey = configuration.GetValue<string>("ApiKey:SecretKey");

            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info.Title = "Simple Api";
                    document.Info.Description = $"Authorization: ApiKey {secretKey}";
                    document.Info.Contact = new OpenApiContact
                    {
                        Name = "Łukasz Kurzyniec",
                        Url = new Uri("https://kurzyniec.pl/"),
                    };
                    return Task.CompletedTask;
                });

                options.AddOperationTransformer<FeatureFlagOperationTransformer>();
                options.AddOperationTransformer<SecurityRequirementOperationTransformer>();

                options.AddDocumentTransformer<RemoveDeprecatedDocumentTransformer>();
                options.AddDocumentTransformer<SecurityDefinitionDocumentTransformer>();
            });
        }
    }
}
