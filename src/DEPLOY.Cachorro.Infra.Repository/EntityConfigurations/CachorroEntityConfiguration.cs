using DEPLOY.Cachorro.Domain;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEPLOY.Cachorro.Infra.Repository.EntityConfigurations
{
    internal class CachorroEntityConfiguration : IEntityTypeConfiguration<Domain.Aggregates.Cachorro.Entities.Cachorro>
    {
        public void Configure(EntityTypeBuilder<Domain.Aggregates.Cachorro.Entities.Cachorro> builder)
        {
            builder
                .ToTable("Cachorro");

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
                .Property(x => x.Cadastro)
                .HasColumnName("DataCadastro")
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");


            builder
                .Property(x => x.Atualizacao)
                .HasColumnName("DataAlteracao")
                .HasColumnType("datetime")
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

            builder
                .Property(x => x.Peso)
                .HasColumnName("Peso")
                .HasColumnType("decimal(6,2)")
                .IsRequired();

            builder
                .Property(x => x.Nascimento)
                .HasColumnName("DataNascimento")
                .HasColumnType("datetime")
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
                .HasOne(x => x.Tutor)
                .WithMany()
                .HasForeignKey("TutorId")
                .IsRequired(false);

            builder.HasIndex(x => x.Adotado);
            builder.HasIndex(x => new { x.Id, x.Atualizacao });

            builder.Ignore(builder => builder.Erros);

            builder.HasData(new Domain.Aggregates.Cachorro.Entities.Cachorro(
                id: Guid.NewGuid(),
                nome: "Rex",
                cadastro: DateTime.Now,
                atualizacao: DateTime.Now,
                nascimento: DateTime.Now,
                adotado: false,
                pelagem: PELAGEM.Curto,
                peso: 10.3F,
                tutor: null));
        }
    }
}