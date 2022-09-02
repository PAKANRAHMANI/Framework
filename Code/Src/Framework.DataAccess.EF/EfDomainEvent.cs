using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Domain;
using Framework.Domain.Events;

namespace Framework.DataAccess.EF
{
    public static class EfDomainEvent 
    {
        public static void Persist(FrameworkDbContext dbContext)
        {
            var aggregateRoots = dbContext.ChangeTracker
                .Entries<IAggregateRoot>()
                .Select(a => a.Entity)
                .ToList();

            var domainEvents = aggregateRoots
                .SelectMany(aggregateRoot => DomainEventStructureFactory.Create(aggregateRoot.GetEvents()))
                .ToList();

            dbContext.DomainEvents.AddRange(domainEvents);
            aggregateRoots.ForEach(a => a.ClearEvents());
        }
    }
}
