using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Specification
{
    public class AndSpecification<T>:CompositeSpecification<T>
    {
        private readonly CompositeSpecification<T> _firstCandidate;
        private readonly CompositeSpecification<T> _secondSpecification;

        public AndSpecification(CompositeSpecification<T> firstSpecification, CompositeSpecification<T> secondSpecification)
        {
            _firstCandidate = firstSpecification;
            _secondSpecification = secondSpecification;
        }
        public override bool IsSatisfiedBy(T candidate)
        {
            return _firstCandidate.IsSatisfiedBy(candidate) && _secondSpecification.IsSatisfiedBy(candidate);
        }
    }
}
