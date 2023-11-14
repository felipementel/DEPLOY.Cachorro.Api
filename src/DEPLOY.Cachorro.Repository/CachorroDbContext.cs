using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DEPLOY.Cachorro.Repository
{
    [ExcludeFromCodeCoverage]
    public class CachorroDbContext : DbContext
    {
        public CachorroDbContext(DbContextOptions<CachorroDbContext> options) :
            base(options)
        {
        }

        public DbSet<Domain.Cachorro> Cachorros { get; set; }

        public DbSet<Domain.Tutor> Tutores { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration<Cachorro.Domain.Cachorro>(new CachorroEntityConfiguration());
            //modelBuilder.ApplyConfiguration<Tutor>(new TutorEntityConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
