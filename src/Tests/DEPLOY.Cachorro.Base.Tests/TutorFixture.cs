using Bogus;
using Bogus.Extensions.Brazil;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Base.Tests
{
    [ExcludeFromCodeCoverage]
    public class TutorFixture
    {
        public List<Domain.Aggregates.Tutor.Entities.Tutor> CreateManyTutores(int quantity)
        {
            return new Faker<Domain.Aggregates.Tutor.Entities.Tutor>(locale: "pt_BR")
                    .CustomInstantiator(t => new Domain.Aggregates.Tutor.Entities.Tutor(
                        t.Random.Long(),
                        t.Person.FirstName,
                        t.Date.Past(),
                        t.Date.Past(),
                        t.Person.Cpf(false)))
                .Generate(quantity);
        }

        public List<Domain.Aggregates.Tutor.Entities.Tutor> CreateManyTutoresWithERRORCPF(int quantity)
        {
            return new Faker<Domain.Aggregates.Tutor.Entities.Tutor>(locale: "pt_BR")
                    .CustomInstantiator(t => new Domain.Aggregates.Tutor.Entities.Tutor(
                        t.Random.Long(),
                        t.Person.FirstName,
                        t.Date.Past(),
                        t.Date.Past(),
                        "12312312356")
                    { Erros = new List<string>(1) { "CPF inválido" } })
                .Generate(quantity);
        }
    }
}
