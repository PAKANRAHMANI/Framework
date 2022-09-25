using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Framework.DataAccess.EF.Mapping;
using Framework.Domain.Events;

namespace Framework.DataAccess.EF
{
    public abstract class FrameworkDbContext : DbContext
    {
        protected FrameworkDbContext() { }
        protected FrameworkDbContext(DbContextOptions options) : base(options) { }
        public DbSet<DomainEventStructure> DomainEvents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DomainEventStructureConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            EfDomainEvent.Persist(this);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            EfDomainEvent.Persist(this);
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            EfDomainEvent.Persist(this);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            EfDomainEvent.Persist(this);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
