using DEPLOY.Cachorro.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEPLOY.Cachorro.Repository.EntityConfigurations
{
    internal class CachorroEntityConfiguration : IEntityTypeConfiguration<Domain.Cachorro>
    {
        public void Configure(EntityTypeBuilder<Domain.Cachorro> builder)
        {
            builder
                .ToTable("Cao");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder
                .Property(x => x.Nome)
                .HasColumnName("Nome")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder
                .Property(x => x.Peso)
                .HasColumnName("Peso")
                .HasColumnType("decimal(6,2)")
                .IsRequired();

            builder
                .Property(x => x.Pelagem)
                .HasConversion(
                    e => e.ToString(),
                    e => (PELAGEM)Enum.Parse(typeof(PELAGEM), e));

            builder
                .Property(x => x.Adotado)
                .HasColumnName("Adotado")
                .HasColumnType("bit")
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

            builder.HasIndex(x => x.Adotado);
            builder.HasIndex(x => new { x.Id, x.Atualizacao });

            builder.HasData(new Domain.Cachorro
            {
                Id = Guid.NewGuid(),
                Nome = "Rex",
                Peso = 9.3F,
                Nascimento = new DateTime(2023, 1, 4, 0, 0, 0, DateTimeKind.Utc),
                Cadastro = System.DateTime.Now,
                Atualizacao = System.DateTime.Now,
                Adotado = true,
                Pelagem = PELAGEM.Longo
            });
        }
    }
}