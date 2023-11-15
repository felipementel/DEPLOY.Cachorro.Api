using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Repository.EntityConfigurations
{
    [ExcludeFromCodeCoverage]
    public class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.UseTpcMappingStrategy();
        }
    }
}