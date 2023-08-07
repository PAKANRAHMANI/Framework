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

            builder.Property<long>("Id")
                .HasColumnName("Id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.HasKey("Id");

        }
    }
}
