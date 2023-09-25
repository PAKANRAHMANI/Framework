using Framework.Core.Events;
using Framework.Core.Filters;

namespace Framework.EventProcessor.Extensions;

public static class OperationExtensions
{
    public static IFilter<IEvent> GetFirstOperation(this IServiceProvider services, Dictionary<int, Type> operations)
    {
        var operationBuilder = FilterBuilder<IEvent>.New();

        foreach (var operation in operations
                     .OrderBy(a => a.Key)
                     .Select(operationKeyValuePair => services.GetService(operationKeyValuePair.Value) as IOperation<IEvent>))
        {
            operationBuilder.WithOperation(operation);
        }

        return operationBuilder.Build();
    }
}