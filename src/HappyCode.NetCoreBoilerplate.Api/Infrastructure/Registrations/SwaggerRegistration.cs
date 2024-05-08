using System.IO;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Registrations
{
    public static class SwaggerRegistration
    {
        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            string secretKey = configuration.GetValue<string>("ApiKey:SecretKey");

            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Simple Api",
                    Version = "v1",
                    Description = $"ApiKey {secretKey}",
                    Contact = new OpenApiContact
                    {
                        Name = "Łukasz Kurzyniec",
                        Url = new Uri("https://kurzyniec.pl/"),
                    }
                });

                swaggerOptions.OrderActionsBy(x => x.RelativePath);
                swaggerOptions.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HappyCode.NetCoreBoilerplate.Api.xml"));

                swaggerOptions.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "ApiKey needed to access the endpoints (eg: `Authorization: ApiKey xxx-xxx`)",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });

                swaggerOptions.OperationFilter<SecurityRequirementSwaggerOperationFilter>();
            });
        }
    }
}
