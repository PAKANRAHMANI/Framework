using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Filters
{
    public interface IFilter<T>
    {
        void SetNext(IFilter<T> next);
        T Apply(T obj);
    }
}
