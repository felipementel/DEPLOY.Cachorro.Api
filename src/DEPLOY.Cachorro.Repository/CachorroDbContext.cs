using DEPLOY.Cachorro.Domain;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

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

        public DbSet<Domain.Tutor> Tutors { get; set; }
    }
}
