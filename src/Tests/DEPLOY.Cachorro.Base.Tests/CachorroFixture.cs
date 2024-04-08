using Bogus;
using Bogus.Extensions.Brazil;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Base.Tests
{
    [ExcludeFromCodeCoverage]
    public class CachorroFixture
    {
        public List<Domain.Aggregates.Cachorro.Entities.Cachorro> CreateManyCachorroWithTutor(int quantity)
        {
            return new Faker<Domain.Aggregates.Cachorro.Entities.Cachorro>(locale: "pt_BR")
                .CustomInstantiator(f => new Domain.Aggregates.Cachorro.Entities.Cachorro(
                    f.Random.Guid(),
                    f.Person.FirstName,
                    f.Date.Past(),
                    f.Date.Past(),
                    f.Date.Past(),
                    true,
                    f.PickRandom<Domain.Aggregates.Cachorro.ValueObject.PELAGEM>(),
                    f.Random.Float(1, 100),
                    new Faker<Domain.Aggregates.Tutor.Entities.Tutor>(locale: "pt_BR")
                    .CustomInstantiator(t => new Domain.Aggregates.Tutor.Entities.Tutor(
                        t.Random.Long(),
                        t.Person.FirstName,
                        f.Date.Past(),
                        f.Date.Past(),
                        f.Person.Cpf(false)))
                    .Generate()))
                .Generate(quantity);
        }

        public List<Domain.Aggregates.Cachorro.Entities.Cachorro> CreateManyCachorroWithoutTutor(int quantity)
        {
            return new Faker<Domain.Aggregates.Cachorro.Entities.Cachorro>(locale: "pt_BR")
                .CustomInstantiator(f => new Domain.Aggregates.Cachorro.Entities.Cachorro(
                    f.Random.Guid(),
                    f.Person.FirstName,
                    f.Date.Past(),
                    f.Date.Past(),
                    f.Date.Past(),
                    true,
                    f.PickRandom<Domain.Aggregates.Cachorro.ValueObject.PELAGEM>(),
                    f.Random.Float(1, 100),
                   null))
                .Generate(quantity);
        }

        public List<Domain.Aggregates.Cachorro.Entities.Cachorro> CreateManyCachorroWithNameError(int quantity)
        {
            var cachorros = new Faker<Domain.Aggregates.Cachorro.Entities.Cachorro>(locale: "pt_BR")
                .CustomInstantiator(f => new Domain.Aggregates.Cachorro.Entities.Cachorro(
                    f.Random.Guid(),
                    f.Person.FirstName,
                    f.Date.Past(),
                    f.Date.Past(),
                    f.Date.Past(),
                    f.Random.Bool(),
                    f.PickRandom<Domain.Aggregates.Cachorro.ValueObject.PELAGEM>(),
                    f.Random.Float(min: 1, max: 100),
                    null))
                .Generate(quantity);

            foreach (var cachorro in cachorros)
            {
                cachorro.Erros.Add("Nome é obrigatório");
            }

            return cachorros;
        }
    }
}
