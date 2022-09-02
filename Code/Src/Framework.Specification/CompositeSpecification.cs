using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Specification
{
    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T candidate);
        public CompositeSpecification<T> And(CompositeSpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public CompositeSpecification<T> Or(CompositeSpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        public CompositeSpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}
