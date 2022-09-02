using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Framework.Domain;

namespace Framework.DataAccess.NH
{
    public abstract class EntityClassMapping<T, TKey> : ClassMapping<T> where T : Entity<TKey>
    {
        protected EntityClassMapping()
        {
            this.Version("RowVersion", a =>
            {
                a.Access(Accessor.Property);
                a.Column("RowVersion");
                a.Type<BinaryTimestamp>();
                a.Generated(VersionGeneration.Always);
            });
            this.Property(a => a.CreationDateTime);
        }
    }
}
