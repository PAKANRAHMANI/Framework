using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Filters
{
    public interface ICondition
    {
        string PropertyName { get; }
        bool IsSatisfied<T>(T obj);
    }
}
