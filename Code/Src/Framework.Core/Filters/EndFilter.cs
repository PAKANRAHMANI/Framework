using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Filters
{
    public class EndFilter : IFilter
    {
        public static IFilter Instance = new EndFilter();
        private EndFilter() { }
        public void SetNext(IFilter next)
        {
            throw new NotSupportedException("Can't set next on EndFilter");
        }

        public T Apply<T>(T obj)
        {
            return obj;
        }
    }
}
