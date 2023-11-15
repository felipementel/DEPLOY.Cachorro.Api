using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain
{
    [ExcludeFromCodeCoverage]
    public class BaseEntity<T>
    {
        public T Id { get; set; } = default!;

        public string Nome { get; set; } = default!;

        public DateTime Cadastro { get; set; }

        public DateTime Atualizacao { get; set; }
    }
}
