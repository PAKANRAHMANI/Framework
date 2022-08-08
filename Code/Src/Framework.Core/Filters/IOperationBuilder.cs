namespace Framework.Core.Filters
{
    public interface IOperationBuilder<T> : IFilterBuilder<T>
    {
        IOperationBuilder<T> WithOperation(IOperation<T> operation);
    }
}