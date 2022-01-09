using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Framework.Domain;

namespace Framework.DataAccess.EF
{
    public abstract class EntityConfiguration<T, TKey> : IEntityTypeConfiguration<T> where T : Entity<TKey>
    {

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .Property(a => a.RowVersion)
                .IsRowVersion();
            builder.Property(a => a.CreationDateTime);
            this.EntityTypeConfiguration(builder);
        }

        public abstract void EntityTypeConfiguration(EntityTypeBuilder<T> builder);
    }
}
