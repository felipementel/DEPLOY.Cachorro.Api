using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain.Shared
{
    [ExcludeFromCodeCoverage]
    public class BaseEntity<Tid>
    {
        public BaseEntity(
            Tid id,
            string nome, 
            DateTime cadastro,
            DateTime? atualizacao)
        {
            Id = id;
            Nome = nome;
            Cadastro = cadastro;
            Atualizacao = atualizacao;
        }

        public BaseEntity(string nome)
        {
            Nome = nome;
        }

        public Tid Id { get; init; }

        public string Nome { get; init; }

        public DateTime Cadastro { get; set; }

        public DateTime? Atualizacao { get; set; }

        public List<string> Erros { get; set; } = new List<string>();
    }
}
