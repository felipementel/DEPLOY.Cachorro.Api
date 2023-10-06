using DEPLOY.Cachorro.Api.Controllers.v1;
using DEPLOY.Cachorro.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Api.Tests
{
    [ExcludeFromCodeCoverage]
    public class CachorrosControllerTest
    {
        [Fact]
        [Trait("Read", "API")]
        public async Task ListarAsync_ReturnOk()
        {
            // Arrange
            var cachorro = new Domain.Cachorro { Nome = "Sirius" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Cachorros.Add(cachorro);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new CachorrosController(context);

                // Act
                var result = await controller.ListarAsync();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.IsType<List<Domain.Cachorro>>(okResult.Value);
            }
        }

        [Fact]
        [Trait("Read", "API")]
        public async Task ObterPorIdAsync_ReturnsOk_WhenCachorroIsFound()
        {
            // Arrange
            var cachorro = new Domain.Cachorro { Nome = "Sirius" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Cachorros.Add(cachorro);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new CachorrosController(context);

                // Act
                var result = await controller.ObterPorIdAsync(cachorro.Id);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var model = Assert.IsType<Domain.Cachorro>(okResult.Value);
                Assert.Equal(cachorro.Id, model.Id);
                Assert.Equal(cachorro.Nome, model.Nome);
            }
        }

        [Fact]
        [Trait("Read", "API")]
        public async Task ObterPorIdAsync_ReturnsNotFound_WhenCachorroNotFound()
        {
            // Arrange
            var cachorro = new Domain.Cachorro { Nome = "Sirius" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Cachorros.Add(cachorro);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new CachorrosController(context);

                // Act
                var result = await controller.ObterPorIdAsync(cachorro.Id + 1);

                // Assert
                Assert.IsType<NotFoundResult>(result);

                using (var contextAfterDelete = new CachorroDbContext(options))
                {
                    var deletedCachorro = await contextAfterDelete.Cachorros.FindAsync(cachorro.Id + 1);
                    Assert.Null(deletedCachorro);
                }
            }
        }

        [Fact]
        [Trait("Create", "API")]
        public async Task CadastrarCachorroAsync_ReturnsCreated_WhenCachorroIsValid()
        {
            // Arrange
            var cachorro = new Domain.Cachorro { Nome = "Sirius" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new CachorrosController(context);

                // Act
                var result = await controller.CadastrarCachorroAsync(cachorro);

                // Assert
                var okResult = Assert.IsType<CreatedAtActionResult>(result);
                var model = Assert.IsType<Domain.Cachorro>(okResult.Value);
                Assert.Equal(cachorro.Nome, model.Nome);
            }
        }

        [Fact]
        [Trait("Update", "API")]
        public async Task PutCachorroAsync_ReturnsNoContent_WhenCachorroIsValid()
        {
            // Arrange
            var cachorro = new Domain.Cachorro { Nome = "Sirius" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Cachorros.Add(cachorro);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new CachorrosController(context);

                // Act
                var result = await controller.PutCachorroAsync(cachorro.Id, new Domain.Cachorro() { Id = cachorro.Id, Nome = "Sirius v2" });

                // Assert
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        [Trait("Update", "API")]
        public async Task PutCachorroAsync_ReturnsBadRequest_WhenCachorroIsInvalid()
        {
            // Arrange
            var cachorro = new Domain.Cachorro { Nome = "Sirius" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Cachorros.Add(cachorro);
                context.SaveChanges();
                //}

                //using (var context = new CachorroDbContext(options))
                //{
                var controller = new CachorrosController(context);

                // Act
                var result = await controller.PutCachorroAsync(2, new Domain.Cachorro() { Nome = "Sirius v2" });

                // Assert
                var okResult = Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        [Trait("Delete", "API")]
        public async Task ExcluirCachorroAsync_ReturnsNotFound_WhenCachorroIdDontExists()
        {
            // Arrange
            var cachorroId = 2;
            var cachorro = new Domain.Cachorro { Id = cachorroId, Nome = "Sirius" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Cachorros.Add(cachorro);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new CachorrosController(context);

                // Act
                var result = await controller.ExcluirCachorroAsync(cachorroId + 1);

                // Assert
                Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);

                using (var contextAfterDelete = new CachorroDbContext(options))
                {
                    var deletedCachorro = await contextAfterDelete.Cachorros.FindAsync(cachorroId);
                    Assert.NotNull(deletedCachorro);
                }
            }
        }

        [Fact]
        [Trait("Delete", "API")]
        public async Task ExcluirCachorroAsync_ReturnsNoContent_WhenCachorroIdIsValid()
        {
            // Arrange
            var cachorro = new Domain.Cachorro { Nome = "Sirius" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Cachorros.Add(cachorro);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new CachorrosController(context);

                // Act
                var result = await controller.ExcluirCachorroAsync(cachorro.Id);

                // Assert
                Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);

                using (var contextAfterDelete = new CachorroDbContext(options))
                {
                    var deletedCachorro = await contextAfterDelete.Cachorros.FindAsync(cachorro.Id);
                    Assert.Null(deletedCachorro);
                }
            }
        }

        [Fact]
        [Trait("Delete", "API")]
        public async Task ExcluirCachorroAsync_ReturnsNoContent_WhenCachorroIsDeleted()
        {
            // Arrange
            var cachorroId = 1;
            var cachorro = new Domain.Cachorro { Id = cachorroId, Nome = "Sirius" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Cachorros.Add(cachorro);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new CachorrosController(context);

                // Act
                var result = await controller.ExcluirCachorroAsync(cachorroId);

                // Assert
                Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);

                using (var contextAfterDelete = new CachorroDbContext(options))
                {
                    var deletedCachorro = await contextAfterDelete.Cachorros.FindAsync(cachorroId);
                    Assert.Null(deletedCachorro);
                }
            }
        }
    }
}