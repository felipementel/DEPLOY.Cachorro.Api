using DEPLOY.Cachorro.Base.Tests;
using DEPLOY.Cachorro.Infra.Repository;
using DEPLOY.Cachorro.Infra.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MockQueryable.Moq;
using Moq;
using Moq.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Repository.Tests
{
    [ExcludeFromCodeCoverage]
    public class TutorRepositoryTest : IClassFixture<TutorFixture>
    {
        private readonly TutorFixture _tutorFixture;

        public TutorRepositoryTest(TutorFixture tutorFixture)
        {
            _tutorFixture = tutorFixture;
        }

        [Fact]
        public async Task GivenGetAllAsync_ReturnsAllTutoresInMemoryDatabase()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var dbContext = new CachorroDbContext(dbContextOptions);
            await dbContext.Database.EnsureCreatedAsync();

            var repository = new Mock<TutorRepository>(dbContext) { CallBase = true };

            var tutores = _tutorFixture.CreateManyTutores(2);

            await dbContext.AddRangeAsync(tutores);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.Object.GetAllAsync();

            // Assert
            Assert.Equal(tutores.Count, result.Count());

            foreach (var tutor in tutores)
            {
                Assert.Contains(result, c => c.Id == tutor.Id);
            }

            dbContext.Database.EnsureDeleted();
        }

        //MockQueryable.Moq
        [Fact]
        public async Task GivenGetAllAsync_WhenRequestIsValidWithMockQueryableMoq_ThenReturnItems()
        {
            // Arrange
            var entities = _tutorFixture.CreateManyTutores(2);
            var dbSetMock = entities.AsQueryable().BuildMockDbSet();

            dbSetMock.Setup(x => x.FindAsync(1))
                .ReturnsAsync(_tutorFixture.CreateManyTutores(1)[0]);

            var dbContextMock = new Mock<CachorroDbContext>(MockBehavior.Strict);
            dbContextMock
                .Setup(c => c.Set<Domain.Aggregates.Tutor.Entities.Tutor>())
                .Returns(dbSetMock.Object);

            var repository = new TutorRepository(dbContextMock.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(entities.Count, result.Count());
        }

        //Moq.EntityFrameworkCore
        [Fact]
        public async Task GivenGetAllAsync_WhenRequestIsValidWithMoqEntityFrameworkCore_ThenReturnItems()
        {
            // Arrange
            var items = _tutorFixture.CreateManyTutores(2);

            var ContextMock = new Mock<CachorroDbContext>();
            ContextMock.Setup<DbSet<Domain.Aggregates.Tutor.Entities.Tutor>>(x => x.Set<Domain.Aggregates.Tutor.Entities.Tutor>())
                .ReturnsDbSet(items);

            var repository = new TutorRepository(ContextMock.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(items.Count, result.Count());

            foreach (var item in items)
            {
                Assert.Contains(result, c => c.Id == item.Id);
            }
        }

        [Fact]
        public async Task GivenGetAllAsync_WhenRequestIsValid_ThenReturnNoItems()
        {
            // Arrange
            var items = _tutorFixture.CreateManyTutores(0);

            var ContextMock = new Mock<CachorroDbContext>();
            ContextMock.Setup<DbSet<Domain.Aggregates.Tutor.Entities.Tutor>>(x => x.Set<Domain.Aggregates.Tutor.Entities.Tutor>())
                .ReturnsDbSet(items);

            var repository = new TutorRepository(ContextMock.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(items.Count, result.Count());
        }
    }
}
