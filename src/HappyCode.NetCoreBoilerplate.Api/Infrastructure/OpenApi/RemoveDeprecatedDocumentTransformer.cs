using Microsoft.OpenApi;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.OpenApi;
using System.Linq;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.OpenApi
{
    /// <summary>
    /// Remove deprecated operations and paths
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class RemoveDeprecatedDocumentTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            foreach (var path in document.Paths)
            {
                var disabledOperations = path.Value.Operations.Where(o => o.Value.Deprecated).ToDictionary();
                if (disabledOperations.Count == 0)
                {
                    continue;
                }

                if (path.Value.Operations.Count == disabledOperations.Count)
                {
                    document.Paths.Remove(path.Key);
                    continue;
                }

                foreach (var operation in disabledOperations)
                {
                    path.Value.Operations.Remove(operation.Key);
                    continue;
                }
            }

            var tags = document.Paths.SelectMany(p => p.Value.Operations.SelectMany(o => o.Value.Tags)).Select(t => t.Target);
            document.Tags.Where(t => !tags.Contains(t)).ToList()
                .Select(t => document.Tags.Remove(t)).ToList();

            return Task.CompletedTask;
        }
    }
}
