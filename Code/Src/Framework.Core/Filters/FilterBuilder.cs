using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Filters
{
    public class FilterBuilder<T> : IOperationBuilder<T>
    {
        private readonly List<IFilter<T>> _filters = new();
        private FilterBuilder() { }
        public static IOperationBuilder<T> New()
        {
            return new FilterBuilder<T>();
        }

        public IOperationBuilder<T> WithOperation(IOperation<T> operation)
        {
            var filter = new Filter<T>(operation);

            _filters.Add(filter);

            return this;
        }

        public IFilter<T> Build()
        {
            if (!_filters.Any()) return EndFilter<T>.Instance;

            this._filters.Aggregate((firstFilter, nextFilter) =>
            {
                firstFilter.SetNext(nextFilter);

                return nextFilter;
            });

            return _filters.First();
        }
    }
}
