using Bogus;
using Bogus.Extensions.Brazil;

namespace DEPLOY.Cachorro.Base.Tests
{
    public class CachorroFixture
    {
        public List<Domain.Aggregates.Cachorro.Entities.Cachorro> CreateManyCachorroWithTutor(int quantity)
        {
            return new Faker<Domain.Aggregates.Cachorro.Entities.Cachorro>()
                .CustomInstantiator(f => new Domain.Aggregates.Cachorro.Entities.Cachorro(
                    f.Random.Guid(),
                    f.Person.FirstName,
                    f.Date.Past(),
                    f.Date.Past(),
                    f.Date.Past(),
                    f.Random.Bool(),
                    f.PickRandom<Domain.Aggregates.Cachorro.ValueObject.PELAGEM>(),
                    f.Random.Float(1, 100),
                    new Faker<Domain.Aggregates.Tutor.Entities.Tutor>()
                    .CustomInstantiator(t => new Domain.Aggregates.Tutor.Entities.Tutor(
                        t.Random.Long(),
                        t.Person.FirstName,
                        f.Date.Past(),
                        f.Date.Past(),
                        long.Parse(f.Person.Cpf().Replace(".", "").Replace("-", ""))))
                    .Generate()))
                .Generate(quantity);
        }
    }
}
