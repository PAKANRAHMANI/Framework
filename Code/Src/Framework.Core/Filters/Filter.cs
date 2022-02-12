namespace Framework.Core.Filters
{
    public class Filter : IFilter
    {
        private readonly ICondition _condition;
        private readonly IOperation _operation;
        private IFilter _nextFilter;

        public Filter(ICondition condition, IOperation operation)
        {
            _condition = condition;
            _operation = operation;
            _nextFilter = EndFilter.Instance;
        }
        public void SetNext(IFilter next)
        {
            this._nextFilter = next;
        }

        public T Apply<T>(T obj)
        {
            if (_condition.IsSatisfied(obj))
                obj = _operation.Apply(obj);

            return _nextFilter.Apply(obj);
        }
    }
}