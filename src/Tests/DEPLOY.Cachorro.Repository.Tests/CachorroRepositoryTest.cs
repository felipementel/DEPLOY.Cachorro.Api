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
    public class CachorroRepositoryTest : IClassFixture<CachorroFixture>
    {
        private readonly CachorroFixture _cachorroFixture;

        public CachorroRepositoryTest(
            CachorroFixture cachorroFixture)
        {
            _cachorroFixture = cachorroFixture;
        }

        [Fact]
        public async Task GivenGetAllAsync_ReturnsAllCachorrosInMemoryDatabase()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var dbContext = new CachorroDbContext(dbContextOptions);
            await dbContext.Database.EnsureCreatedAsync();

            var repository = new Mock<CachorroRepository>(dbContext) { CallBase = true };

            var cachorros = _cachorroFixture.CreateManyCachorroWithTutor(2);

            await dbContext.AddRangeAsync(cachorros);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.Object.GetAllAsync();

            // Assert
            Assert.Equal(cachorros.Count + 1, result.Count());

            foreach (var cachorro in cachorros)
            {
                Assert.Contains(result, c => c.Id == cachorro.Id);
            }

            dbContext.Database.EnsureDeleted();
        }

        //MockQueryable.Moq
        [Fact]
        public async Task GivenGetAllAsync_WhenRequestIsValidWithMockQueryableMoq_ThenReturnItems()
        {
            // Arrange
            var entities = _cachorroFixture.CreateManyCachorroWithTutor(2);
            var dbSetMock = entities.AsQueryable().BuildMockDbSet();

            dbSetMock.Setup(x => x.FindAsync(1))
                .ReturnsAsync(_cachorroFixture.CreateManyCachorroWithTutor(1)[0]);

            var dbContextMock = new Mock<CachorroDbContext>(MockBehavior.Strict);
            dbContextMock
                .Setup(c => c.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
                .Returns(dbSetMock.Object);

            var repository = new CachorroRepository(dbContextMock.Object);

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
            var items = _cachorroFixture.CreateManyCachorroWithTutor(2);

            var ContextMock = new Mock<CachorroDbContext>();
            ContextMock.Setup<DbSet<Domain.Aggregates.Cachorro.Entities.Cachorro>>(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
                .ReturnsDbSet(items);

            var repository = new CachorroRepository(ContextMock.Object);

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
            var items = _cachorroFixture.CreateManyCachorroWithTutor(0);

            var ContextMock = new Mock<CachorroDbContext>();
            ContextMock.Setup<DbSet<Domain.Aggregates.Cachorro.Entities.Cachorro>>(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
                .ReturnsDbSet(items);

            var repository = new CachorroRepository(ContextMock.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(items.Count, result.Count());
        }

        [Fact]
        public async Task GivenGetByIdAsync_WhenCachorroExists_ThanReturnSuccess()
        {
            // Arrange
            var items = _cachorroFixture.CreateManyCachorroWithTutor(1);

            var ContextMock = new Mock<CachorroDbContext>();

            ContextMock.Setup<DbSet<Domain.Aggregates.Cachorro.Entities.Cachorro>>(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
                .ReturnsDbSet(items);

            var repository = new CachorroRepository(ContextMock.Object);

            // Act
            var item = await repository.GetByIdAsync(items[0].Id, CancellationToken.None);

            // Assert
            Assert.NotNull(item);
        }

        [Fact]
        public async Task GivenInsertAsync_WhenCachorroIsValid_ThanReturnSuccess()
        {
            // Arrange
            var items = _cachorroFixture.CreateManyCachorroWithTutor(1);

            var ContextMock = new Mock<CachorroDbContext>();
            ContextMock.Setup<DbSet<Domain.Aggregates.Cachorro.Entities.Cachorro>>(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
                .ReturnsDbSet(items);

            var repository = new CachorroRepository(ContextMock.Object);

            // Act
            var item = await repository.InsertAsync(items[0], CancellationToken.None);

            // Assert
            Assert.NotNull(item);

        }

        [Fact]
        public async Task GivenInsertAsync_WhenCachorroIsNotValid_ThanReturnException()
        {
            // Arrange
            var items = _cachorroFixture.CreateManyCachorroWithNameError(1);

            var ContextMock = new Mock<CachorroDbContext>();
            ContextMock.Setup<DbSet<Domain.Aggregates.Cachorro.Entities.Cachorro>>(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
                .ReturnsDbSet(items);

            var repository = new CachorroRepository(ContextMock.Object);

            // Act
            var item = await repository.InsertAsync(It.IsAny<Domain.Aggregates.Cachorro.Entities.Cachorro>(), CancellationToken.None);

            // Assert
            Assert.Null(item);
        }

        [Fact]
        public async Task GivenUpdateAsync_WhenCachorroExists_ThanSuccess()
        {
            // Arrange
            var entities = _cachorroFixture.CreateManyCachorroWithoutTutor(1);

            var dbContextOptions = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var dbContext = new CachorroDbContext(dbContextOptions);
            //await dbContext.Database.EnsureCreatedAsync();

            var repository = new Mock<CachorroRepository>(dbContext) { CallBase = true };

            var cachorros = _cachorroFixture.CreateManyCachorroWithTutor(2);

            await dbContext.AddRangeAsync(cachorros);
            await dbContext.SaveChangesAsync();

            // Act
            await repository.Object.UpdateAsync(entities[0], default);

            // Assert
            Assert.Equal(EntityState.Modified, dbContext.Entry(entities[0]).State);

            dbContext.Database.EnsureDeleted();
        }

        //[Fact]
        //public async Task GivenUpdateAsync_WhenCachorroExists_ThanSuccess()
        //{
        //    // Assume you have an existing entity 'Cachorro'
        //    var entities = _cachorroFixture.CreateManyCachorroWithoutTutor(1);
        //    var dbSetMock = entities.AsQueryable().BuildMockDbSet();

        //    dbSetMock.Setup(x => x.FindAsync(1))
        //        .ReturnsAsync(entities[0]);

        //    var dbContextMock = new Mock<CachorroDbContext>(MockBehavior.Strict);
        //    var stateManager = new Mock<IStateManager>();
        //    var changeDetector = new Mock<IChangeDetector>();
        //    var model = new Mock<IModel>();
        //    var graphIterator = new Mock<IEntityEntryGraphIterator>();
        //    var mockEntry = new Mock<EntityEntry<Domain.Aggregates.Cachorro.Entities.Cachorro>>();

        //    dbSetMock
        //        .Setup(x => x.FindAsync(1))
        //        .ReturnsAsync(_cachorroFixture.CreateManyCachorroWithTutor(1)[0]);

        //    dbContextMock
        //        .Setup(c => c.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
        //        .Returns(dbSetMock.Object);

        //    //var changeTracker = new Mock<ChangeTracker>();

        //    //dbContextMock
        //    //    .Setup(ct => ct.ChangeTracker)
        //    //    .Returns(new ChangeTracker(dbContextMock.Object, stateManager.Object, changeDetector.Object, model.Object, graphIterator.Object));

        //    dbContextMock
        //        .Setup<DbSet<Domain.Aggregates.Cachorro.Entities.Cachorro>>(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
        //        .ReturnsDbSet(entities);

        //    dbContextMock
        //        .Setup(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>()
        //        .FindAsync(new object[] { entities[0].Id }, default))
        //        .ReturnsAsync(entities[0]);

        //    // Create a custom EntityEntry mock
        //    mockEntry
        //        .Setup(e => e.State)
        //        .Returns(EntityState.Modified);

        //    mockEntry
        //        .Setup(e => e.Entity)
        //        .Returns(entities[0]); // Set the tracked entity         


        //    dbContextMock.Setup(c => c.Entry(entities[0]))
        //        .Returns(mockEntry.Object);

        //    var repository = new CachorroRepository(dbContextMock.Object);

        //    // Act
        //    await repository.UpdateAsync(entities[0], CancellationToken.None);

        //    // Assert
        //    dbContextMock.Verify(x => x.Update(entities[0]), Times.Once);
        //    dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Never);
        //}

        [Fact]
        public async Task GivenDeleteAsync_WhenCachorroExists_ThenReturnTrue()
        {
            // Arrange
            var items = _cachorroFixture.CreateManyCachorroWithoutTutor(1);

            var ContextMock = new Mock<CachorroDbContext>();

            ContextMock
                .Setup<DbSet<Domain.Aggregates.Cachorro.Entities.Cachorro>>(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
                .ReturnsDbSet(items);

            ContextMock
                .Setup(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>()
                .FindAsync(new object[] { items[0].Id }, default))
                .ReturnsAsync(items[0]);

            var repository = new CachorroRepository(ContextMock.Object);

            // Act
            var result = await repository.DeleteAsync(items[0].Id, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GivenDeleteAsync_WhenCachorroNotExist_ThenReturnFalse()
        {
            // Arrange
            var items = _cachorroFixture.CreateManyCachorroWithoutTutor(1);

            var ContextMock = new Mock<CachorroDbContext>();

            ContextMock
                .Setup<DbSet<Domain.Aggregates.Cachorro.Entities.Cachorro>>(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
                .ReturnsDbSet(items);

            ContextMock
                .Setup(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>()
                .FindAsync(new object[] { items[0].Id }, default))
                .ReturnsAsync(null as Domain.Aggregates.Cachorro.Entities.Cachorro);

            var repository = new CachorroRepository(ContextMock.Object);

            // Act
            var result = await repository.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GivenGetByKeyAsync_WhenExitsCachorros_ThanCompareIdsAndFoundObject()
        {
            // Arrange
            int qtdCachorros = 10;
            var cachorros = _cachorroFixture.CreateManyCachorroWithoutTutor(qtdCachorros);

            var ContextMock = new Mock<CachorroDbContext>();

            ContextMock
                .Setup<DbSet<Domain.Aggregates.Cachorro.Entities.Cachorro>>(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
                .ReturnsDbSet(cachorros);

            var repository = new CachorroRepository(ContextMock.Object);

            int indiceAleatorio = new Random().Next(0, cachorros.Count -1);

            // Acessar o item correspondente ao índice aleatório na lista
            var itemAleatorio = cachorros[indiceAleatorio];

            // Act
            var result = await repository.GetByKeyAsync(x => x.Id == itemAleatorio.Id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(itemAleatorio.Id, result[0].Id);
        }
    }
}
