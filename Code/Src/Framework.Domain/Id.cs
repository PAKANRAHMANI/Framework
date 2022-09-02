using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Domain
{
    public class Id<TKey> : ValueObject
    {
        public TKey DbId { get; protected set; }

        protected bool Equals(Id<TKey> other)
        {
            return base.Equals(other) && EqualityComparer<TKey>.Default.Equals(DbId, other.DbId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Id<TKey>) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), DbId);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DbId;
        }
    }
}
