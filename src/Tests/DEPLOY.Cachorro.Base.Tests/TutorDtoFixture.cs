using Bogus;
using Bogus.Extensions.Brazil;
using DEPLOY.Cachorro.Application.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Base.Tests
{
    [ExcludeFromCodeCoverage]
    public class TutorDtoFixture
    {
        public List<TutorDto> CreateManyTutorDto(int quantity)
        {
            return new Faker<TutorDto>(locale: "pt_BR")
                    .CustomInstantiator(t => new TutorDto(
                        Id: t.Random.Long(0, long.MaxValue),
                        Nome: t.Person.FirstName,
                        Cadastro: DateTime.Now,
                        Atualizacao: t.Date.Past(),
                        CPF: t.Person.Cpf(false),
                        Erros: Enumerable.Empty<string>()))
                .Generate(quantity);
        }

        public List<TutorDto> CreateManyTutorDtoWithNameError(int quantity)
        {
            return new Faker<TutorDto>(locale: "pt_BR")
                    .CustomInstantiator(t => new TutorDto(
                        Id: t.Random.Long(0, long.MaxValue),
                        Nome: string.Empty,
                        Cadastro: DateTime.Now,
                        Atualizacao: t.Date.Past(),
                        CPF: t.Person.Cpf(false),
                        Erros: Enumerable.Empty<string>().Append("Nome é obrigatório")))
                .Generate(quantity);
        }
    }
}
