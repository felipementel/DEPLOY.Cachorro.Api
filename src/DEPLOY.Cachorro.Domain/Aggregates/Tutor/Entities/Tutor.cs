﻿using System.Diagnostics.CodeAnalysis;
using DEPLOY.Cachorro.Domain.Shared;
namespace DEPLOY.Cachorro.Domain.Aggregates.Tutor.Entities
{
    [ExcludeFromCodeCoverage]
    public class Tutor : BaseEntity<long>
    {
        public Tutor(
            long id,
            string nome,
            DateTime cadastro,
            DateTime? atualizacao,
            string cPF) : base(id, nome, cadastro, atualizacao)
        {
            Id = id;
            Nome = nome;
            Cadastro = cadastro;
            Atualizacao = atualizacao;
            CPF = cPF;
        }

        public string CPF { get; set; }
    }
}
