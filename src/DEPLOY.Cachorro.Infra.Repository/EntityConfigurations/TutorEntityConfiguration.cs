using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Infra.Repository.EntityConfigurations
{
    [ExcludeFromCodeCoverage]
    internal class TutorEntityConfiguration : IEntityTypeConfiguration<Tutor>
    {
        public void Configure(EntityTypeBuilder<Tutor> builder)
        {
            builder
                .ToTable("Tutor");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .UseIdentityColumn()
                .HasColumnName("Id")
                .HasColumnType("bigint")
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
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("getdate()")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


            builder
                .Property(x => x.Atualizacao)
                .HasColumnName("DataAlteracao")
                .HasColumnType("datetime")
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

            builder.Ignore(builder => builder.Erros);

            builder.HasIndex(x => new { x.Id, x.Atualizacao });
        }
    }
}