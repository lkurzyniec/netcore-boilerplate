using Microsoft.OpenApi;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.OpenApi
{
    /// <summary>
    /// Mark disabled features as Deprecated
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FeatureFlagOperationTransformer(IFeatureManager featureManager) : IOpenApiOperationTransformer
    {
        public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            var featureGates = context.Description.ActionDescriptor.EndpointMetadata.OfType<FeatureGateAttribute>();
            if (!featureGates.Any())
            {
                return;
            }

            var features = featureGates.SelectMany(f => f.Features).Distinct();
            bool isDisabled = (await Task.WhenAll(features.Select(feature => featureManager.IsEnabledAsync(feature)))).Any(enabled => !enabled);
            if (isDisabled)
            {
                operation.Deprecated = true;
            }
        }
    }
}
