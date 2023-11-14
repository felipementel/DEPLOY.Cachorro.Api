using DEPLOY.Cachorro.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEPLOY.Cachorro.Repository.EntityConfigurations
{
    public class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity<Guid>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.UseTpcMappingStrategy();
        }
    }
}