using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Specification
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T candidate);
    }
}
