using System;
using System.Collections.Generic;
using Framework.Core.Utilities;

namespace Framework.Domain
{
    public abstract class Entity<TKey> : IEntity
    {
        public byte[] RowVersion { get; protected set; }
        public DateTime CreationDateTime { get; protected set; }
        public DateTime? LastUpdateDateTime { get; private set; }
        public DateTime? DeleteDateTime { get; private set; }
        public bool IsDeleted { get; private set; }
        public TKey Id { get; protected set; }

        protected Entity()
        {
            this.CreationDateTime = DateTime.UtcNow;
            this.IsDeleted = false;
        }
        protected bool Equals(Entity<TKey> other)
        {
            return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Entity<TKey>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Id);
        }

        public void MarkAsRowVersion()
        {
            this.RowVersion = DateTimeOffset.UtcNow.GetBytes();
        }

        public void MarkAsUpdated()
        {
           this.LastUpdateDateTime = DateTime.UtcNow;
        }

        public void MarkAsDeleted()
        {
            this.IsDeleted = true;
            this.DeleteDateTime = DateTime.UtcNow;
        }
    }
}