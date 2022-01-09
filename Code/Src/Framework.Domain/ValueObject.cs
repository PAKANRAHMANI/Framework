using Framework.Core;
using Framework.Core.Equality;

namespace Framework.Domain
{
    public class ValueObject : IValueObject
    {
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            return EqualsBuilder.ReflectionEquals(this, obj);
        }
        public override int GetHashCode()
        {
            return HashCodeBuilder.ReflectionHashCode(this);
        }
    }
}