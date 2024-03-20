using Bogus;
using Bogus.Extensions.Brazil;
using DEPLOY.Cachorro.Application.Dtos;

namespace DEPLOY.Cachorro.Base.Tests
{
    public class CachorroDtoFixture
    {
        public List<CachorroDto> CreateManyCachorroDtoWithTutorDto(int quantity)
        {
            return new Faker<CachorroDto>()
                .CustomInstantiator(f => new CachorroDto(
                    f.Random.Guid(),
                    f.Person.FirstName,
                    f.Date.Past(),
                    f.Date.Past(),
                    f.Date.Past(),
                    f.Random.Bool(),
                    f.PickRandom<Domain.Aggregates.Cachorro.ValueObject.PELAGEM>(),
                    f.Random.Float(1, 100),
                    new Faker<TutorDto>()
                    .CustomInstantiator(t => new TutorDto(
                        t.Random.Long(),
                        t.Person.FirstName,
                        f.Date.Past(),
                        f.Date.Past(),
                        long.Parse(f.Person.Cpf().Replace(".", "").Replace("-", ""))))
                    .Generate()))
                .Generate(quantity);
        }

        public List<CachorroCreateDto> CreateManyCachorroCreatedDto(int quantity)
        {
            return new Faker<CachorroCreateDto>()
                .CustomInstantiator(f => new CachorroCreateDto(
                    f.Person.FirstName,
                    f.Date.Past(),
                    f.PickRandom<Domain.Aggregates.Cachorro.ValueObject.PELAGEM>(),
                    f.Random.Float(1, 100)))
                .Generate(quantity);
        }

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
