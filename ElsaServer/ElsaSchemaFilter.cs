using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ElsaServer
{
    public class ElsaSchemaFilter: ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            // Skip problematic Elsa types that cause serialization issues
            if (type.Namespace?.StartsWith("Elsa") == true)
            {
                // Handle specific problematic properties
                var problematicProperties = new[]
                {
                "Parent", "Root", "NodeId", "Activity", "WorkflowGraph", "Metadata"
            };

                foreach (var prop in problematicProperties)
                {
                    if (schema.Properties?.ContainsKey(prop.ToLowerInvariant()) == true)
                    {
                        schema.Properties.Remove(prop.ToLowerInvariant());
                    }
                }
            }

            // Handle JsonElement properties that cause issues
            if (type == typeof(System.Text.Json.JsonElement) ||
                type == typeof(System.Text.Json.JsonElement?))
            {
                schema.Type = "object";
                schema.Properties = null;
                schema.AdditionalPropertiesAllowed = true;
            }

            // Handle object types that might contain circular references
            if (type == typeof(object))
            {
                schema.Type = "object";
                schema.Properties = null;
                schema.AdditionalPropertiesAllowed = true;
            }
        }
    }
}
