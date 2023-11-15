using DEPLOY.Cachorro.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEPLOY.Cachorro.Repository.EntityConfigurations
{
    internal class TutorEntityConfiguration : IEntityTypeConfiguration<Domain.Tutor>
    {
        public void Configure(EntityTypeBuilder<Domain.Tutor> builder)
        {
            builder
                .ToTable("Tutor");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("int")
                .IsRequired();

            builder
                .Property(x => x.CPF)
                .HasColumnName("CPF")
                .HasColumnType("varchar(11)")
                .IsRequired();

            builder
                .Property(x => x.Nome)
                .HasColumnName("Nome")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder
                .Property(x => x.Cadastro)
                .HasColumnName("DataCadastro")
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(x => x.Atualizacao)
                .HasColumnName("DataAlteracao")
                .HasColumnType("datetime")
                .IsRequired();

            builder.HasIndex(x => new { x.Id, x.Atualizacao });
        }
    }
}