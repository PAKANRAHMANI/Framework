using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Filters
{
    public class EndFilter<T> : IFilter<T>
    {
        public static IFilter<T> Instance = new EndFilter<T>();
        private EndFilter() { }
        public void SetNext(IFilter<T> next)
        {
            throw new NotSupportedException("Can't set next on EndFilter");
        }

        public async Task<T> Apply(T obj)
        {
            return await Task.FromResult(obj);
        }
    }
}
