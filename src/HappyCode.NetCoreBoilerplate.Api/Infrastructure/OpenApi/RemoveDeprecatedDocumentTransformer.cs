using Microsoft.OpenApi;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.OpenApi;

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
            List<string> expectedModels = ["HttpValidationProblemDetails"];
            List<string> modelsToRemove = [];
            foreach (var path in document.Paths)
            {
                var disabledOperations = path.Value.Operations.Where(o => o.Value.Deprecated).ToDictionary();
                if (disabledOperations.Count == 0)
                {
                    expectedModels.AddRange(path.Value.Operations.SelectMany(o => GetModels(o.Value)));
                    continue;
                }

                if (path.Value.Operations.Count == disabledOperations.Count)
                {
                    modelsToRemove.AddRange(disabledOperations.SelectMany(o => GetModels(o.Value)));
                    document.Paths.Remove(path.Key);
                    continue;
                }

                foreach (var operation in disabledOperations)
                {
                    modelsToRemove.AddRange(GetModels(operation.Value));
                    path.Value.Operations.Remove(operation.Key);
                    continue;
                }
            }

            RemoveTags(document);
            RemoveModels(document, modelsToRemove.Except(expectedModels));

            return Task.CompletedTask;
        }

        private static void RemoveTags(OpenApiDocument document)
        {
            var tags = document.Paths.SelectMany(p => p.Value.Operations.SelectMany(o => o.Value.Tags)).Select(t => t.Target);
            document.Tags.Where(t => !tags.Contains(t)).ToList().ForEach(t => document.Tags.Remove(t));
        }

        private static IEnumerable<string> GetModels(OpenApiOperation operation)
        {
            var referencedComponents =
                operation.Responses
                .Where(r => r.Value.Content.Count > 0)
                .Select(r => r.Value.Content.First().Value.Schema?.Items as OpenApiSchemaReference);

            referencedComponents = referencedComponents.Concat(
                operation.Responses
                .Where(r => r.Value.Content.Count > 0)
                .Select(r => r.Value.Content.First().Value.Schema as OpenApiSchemaReference)
            );

            referencedComponents = referencedComponents.Concat(
                [operation.RequestBody?.Content.First().Value.Schema as OpenApiSchemaReference]
            );

            referencedComponents = referencedComponents.Where(r => r is not null);

            if (!referencedComponents.Any())
            {
                return [];
            }

            return referencedComponents.Select(r => r.Reference.Id);
        }

        private static void RemoveModels(OpenApiDocument document, IEnumerable<string> modelsToRemove)
        {
            var nestedModels = modelsToRemove.SelectMany(m => document.Components.Schemas[m].Properties.Select(p => p.Value as OpenApiSchemaReference))
                .Where(r => r is not null)
                .Select(r => r.Reference.Id)
                .ToList();
            modelsToRemove
                .Concat(nestedModels)
                .Distinct()
                .Select(document.Components.Schemas.Remove).ToList();
        }
    }
}
