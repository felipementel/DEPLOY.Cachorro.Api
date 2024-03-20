using DEPLOY.Cachorro.Base.Tests;
using DEPLOY.Cachorro.Infra.Repository;
using DEPLOY.Cachorro.Infra.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MockQueryable.Moq;
using Moq;
using Moq.EntityFrameworkCore;

namespace DEPLOY.Cachorro.Repository.Tests
{
    public class CachorroRepositoryTest : IClassFixture<CachorroFixture>
    {
        private readonly CachorroFixture _cachorroFixture;
        public CachorroRepositoryTest()
        {
            _cachorroFixture = new();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCachorrosInMemoryDatabase()
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

        ////MockQueryable.Moq
        //[Fact]
        //public async Task GetAllAsync_ReturnsAllCachorrosMoqDatabase()
        //{
        //    // Arrange
        //    var entities = _cachorroFixture.CreateManyCachorroWithTutor(2);
        //    var dbSetMock = entities.AsQueryable().BuildMockDbSet();

        //    dbSetMock.Setup(x => x.FindAsync(1))
        //        .ReturnsAsync(_cachorroFixture.CreateManyCachorroWithTutor(1)[0]);

        //    var dbContextMock = new Mock<CachorroDbContext>(MockBehavior.Strict);
        //    dbContextMock
        //        .Setup(c => c.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
        //        .Returns(dbSetMock.Object);

        //    var repository = new CachorroRepository(dbContextMock.Object);

        //    // Act
        //    var result = await repository.GetAllAsync();

        //    // Assert
        //    Assert.Equal(entities.Count, result.Count());
        //}

        //[Fact]
        //public async Task AAAAGetAllAsync_ReturnsAllCachorrosMoqDatabase()
        //{
        //    // Arrange
        //    var entities = _cachorroFixture.CreateManyCachorroWithTutor(2);
        //    var dbSetMock = entities.AsQueryable().BuildMockDbSet();

        //    // Configurar o mock do DbSet para retornar um cachorro específico ao chamar FindAsync
        //    dbSetMock.Setup(x => x.FindAsync(1))
        //        .ReturnsAsync(_cachorroFixture.CreateManyCachorroWithTutor(1).First()); // Retorna o primeiro cachorro da lista

        //    // Criar um mock de DbContextOptionsBuilder<CachorroDbContext>
        //    var optionsBuilderMock = new Mock<DbContextOptionsBuilder<CachorroDbContext>>();

        //    // Criar um DbContextOptions válido
        //    var dbContextOptions = new DbContextOptions<CachorroDbContext>();

        //    // Configurar o comportamento do mock para retornar o DbContextOptions criado
        //    optionsBuilderMock.Setup(x => x.Options).Returns(dbContextOptions);

        //    // Criar o mock do DbContext usando o DbContextOptions
        //    var dbContextMock = new Mock<CachorroDbContext>(dbContextOptions, MockBehavior.Strict);

        //    // Configurar o mock do contexto do banco de dados para retornar o mock do DbSet
        //    dbContextMock
        //        .Setup(c => c.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
        //        .Returns(dbSetMock.Object);

        //    // Criar o repositório usando o mock do contexto do banco de dados
        //    var repository = new CachorroRepository(dbContextMock.Object);

        //    // Act
        //    var result = await repository.GetAllAsync();

        //    // Assert
        //    Assert.Equal(entities.Count, result.Count());
        //}


        ////Moq.EntityFrameworkCore
        //[Fact]
        //public async Task method()
        //{
        //    // Arrange
        //    var items = _cachorroFixture.CreateManyCachorroWithTutor(2);

        //    var ContextMock = new Mock<CachorroDbContext>();
        //    ContextMock.Setup<DbSet<Domain.Aggregates.Cachorro.Entities.Cachorro>>(x => x.Set<Domain.Aggregates.Cachorro.Entities.Cachorro>())
        //        .ReturnsDbSet(items);

        //    var repository = new CachorroRepository(ContextMock.Object);

        //    // Act
        //    var result = await repository.GetAllAsync();

        //    // Assert
        //    Assert.Equal(items.Count, result.Count());

        //    foreach (var item in items)
        //    {
        //        Assert.Contains(result, c => c.Id == item.Id);
        //    }
        //}
    }
}
