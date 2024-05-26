using System.Linq;
using HappyCode.NetCoreBoilerplate.Core;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters
{
    public class FeatureFlagSwaggerDocumentFilter : IDocumentFilter
    {
        private readonly IFeatureManager _featureManager;

        public FeatureFlagSwaggerDocumentFilter(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var featureFlags = new Dictionary<string, bool>();
            foreach (var item in Enum.GetNames<FeatureFlags>())
            {
                featureFlags.Add(item, _featureManager.IsEnabledAsync(item).GetAwaiter().GetResult());
            }

            foreach (var apiDescription in context.ApiDescriptions)
            {
                var featureGates = apiDescription.CustomAttributes().OfType<FeatureGateAttribute>();
                foreach (var feature in featureGates.SelectMany(x => x.Features))
                {
                    if (!featureFlags[feature])
                    {
                        var route = "/" + apiDescription.RelativePath.TrimEnd('/');
                        swaggerDoc.Paths.Remove(route);
                        continue;
                    }
                }

            }
        }
    }
}
