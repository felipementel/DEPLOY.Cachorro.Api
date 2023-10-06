﻿using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain
{
    [ExcludeFromCodeCoverage]
    public  class BaseEntidade<T>
    {
        public T Id { get; set; }

        public string Nome { get; set; }

        public DateTime Cadastro { get; set; }

        public DateTime Atualizacao { get; set; }
    }
}
