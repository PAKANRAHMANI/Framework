using Framework.DataAccess.EF;
using Microsoft.EntityFrameworkCore;

namespace Framework.Testing.Core.Persist
{
    internal class TestDbContext : FrameworkDbContext
    {
        public TestDbContext(DbContextOptions options):base(options)
        {
            
        }
    }
}