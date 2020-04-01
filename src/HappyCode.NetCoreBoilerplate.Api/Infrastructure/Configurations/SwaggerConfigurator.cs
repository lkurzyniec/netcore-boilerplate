using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations
{
    public static class SwaggerConfigurator
    {
        public static void Configure(SwaggerGenOptions swaggerOptions, string secretKey)
        {
            swaggerOptions.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Simple Api",
                Version = "v1",
                Description = $"ApiKey {secretKey}",
                Contact = new OpenApiContact
                {
                    Name = "Łukasz Kurzyniec",
                    Url = new Uri("https://kurzynieswaggerOptions.pl/"),
                }
            });

            swaggerOptions.DescribeAllParametersInCamelCase();
            swaggerOptions.OrderActionsBy(x => x.RelativePath);

            swaggerOptions.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "ApiKey needed to access the endpoints (eg: `Authorization: ApiKey xxx-xxx`)",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
            });

            swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
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
