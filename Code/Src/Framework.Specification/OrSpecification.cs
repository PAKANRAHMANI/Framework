using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Specification
{
    public class OrSpecification<T> : CompositeSpecification<T>
    {
        private readonly CompositeSpecification<T> _firstSpecification;
        private readonly CompositeSpecification<T> _secondSpecification;

        public OrSpecification(CompositeSpecification<T> firstSpecification, CompositeSpecification<T> secondSpecification)
        {
            _firstSpecification = firstSpecification;
            _secondSpecification = secondSpecification;
        }
        public override bool IsSatisfiedBy(T candidate)
        {
            return _firstSpecification.IsSatisfiedBy(candidate) || _secondSpecification.IsSatisfiedBy(candidate);
        }
    }
}
