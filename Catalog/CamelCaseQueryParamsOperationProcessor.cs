using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Catalog;
public class CamelCaseQueryParamsOperationProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        foreach (var item in context.OperationDescription.Operation.Parameters.Skip(1))
        {
            item.Name = char.ToLowerInvariant(item.Name[0]) + item.Name[1..];
        }

        if (context.OperationDescription.Operation.RequestBody?.Content != null)
        {
            foreach (var (key, item) in context.OperationDescription.Operation.RequestBody.Content)
            {
                var propsToChange = new Dictionary<string, JsonSchemaProperty>();
                foreach (var prop in item.Schema.Properties)
                {
                    propsToChange.Add(char.ToLowerInvariant(prop.Key[0]) + prop.Key[1..], prop.Value);
                }
                item.Schema.Properties.Clear();
                foreach (var prop in propsToChange)
                {
                    item.Schema.Properties.Add(prop.Key, prop.Value);
                }
            }
        }
        return true;
    }
}
