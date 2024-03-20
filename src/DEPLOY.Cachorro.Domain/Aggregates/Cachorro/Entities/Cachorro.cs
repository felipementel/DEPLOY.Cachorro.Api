using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.ValueObject;
using DEPLOY.Cachorro.Domain.Shared;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Entities
{
    [ExcludeFromCodeCoverage]
    public class Cachorro : BaseEntity<Guid>
    {
        public Cachorro(
            Guid id,
            string nome,
            DateTime cadastro,
            DateTime? atualizacao,
            DateTime nascimento,
            bool adotado,
            PELAGEM pelagem,
            float peso,
            Tutor.Entities.Tutor tutor) : base(id, nome, cadastro, atualizacao)
        {
            Id = id;
            Nome = nome;
            Cadastro = cadastro;
            Atualizacao = atualizacao;
            Nascimento = nascimento;
            Adotado = adotado;
            Pelagem = pelagem;
            Peso = peso;
        }

        public Cachorro(
            Guid id,
            string nome,
            DateTime cadastro,
            DateTime? atualizacao,
            DateTime nascimento,
            bool adotado,
            PELAGEM pelagem,
            float peso) : base(id, nome, cadastro, atualizacao)
        {
            Id = id;
            Nome = nome;
            Cadastro = cadastro;
            Atualizacao = atualizacao;
            Nascimento = nascimento;
            Adotado = adotado;
            Pelagem = pelagem;
            Peso = peso;
        }

        public Cachorro(
                       string nome,
                       DateTime nascimento,
                       PELAGEM pelagem,
                       float peso) : base(nome)
        {
            Nome = nome;
            Nascimento = nascimento;
            Pelagem = pelagem;
            Peso = peso;
        }

        public DateTime Nascimento { get; private set; }

        public bool Adotado { get; set; } = false;

        public PELAGEM Pelagem { get; init; }

        public float Peso { get; init; }

        public Tutor.Entities.Tutor? Tutor { get; set; }

        public void Adotar(Tutor.Entities.Tutor tutor)
        {
            Atualizacao = DateTime.Now;
            Tutor = tutor;
            Adotado = true;
        }

        internal void UpdateNascimento(DateTime nascimento)
        {
            Nascimento = nascimento;
        }
    }
}
