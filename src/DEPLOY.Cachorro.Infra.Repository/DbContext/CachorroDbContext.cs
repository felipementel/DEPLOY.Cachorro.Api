using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DEPLOY.Cachorro.Infra.Repository
{
    [ExcludeFromCodeCoverage]
    public class CachorroDbContext : DbContext
    {
        public CachorroDbContext(DbContextOptions<CachorroDbContext> options) :
            base(options)
        {
        }

        public CachorroDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
