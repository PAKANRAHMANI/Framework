namespace Framework.Core.Filters
{
    public interface IFilterBuilder<T>
    {
        IFilter<T> Build();
    }
}