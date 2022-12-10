using System;
using System.Collections.Generic;
using System.Text;
using Framework.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DataAccess.EF.Mapping
{
    public class DomainEventStructureConfiguration : IEntityTypeConfiguration<DomainEventStructure>
    {
        public void Configure(EntityTypeBuilder<DomainEventStructure> builder)
        {
            builder.ToTable("DomainEvents");
            builder.HasKey(a => a.EventId);
            builder.Property(a=>a.AggregateType)
                .HasConversion(b=>b.FullName,c=>Type.GetType(c));
        }
    }
}
