using DEPLOY.Cachorro.Api.Controllers.v1;
using DEPLOY.Cachorro.Repository;
using FluentAssertions;
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
                result.Should().BeOfType<OkObjectResult>();
                result.As<OkObjectResult>().Value
                    .Should().BeOfType<List<Domain.Cachorro>>();
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
                result.Should().BeOfType<OkObjectResult>();
                var model = result.As<OkObjectResult>().Value
                    .Should().BeOfType<Domain.Cachorro>();
                model.Subject.Id.Should().Be(cachorro.Id);
                model.Subject.Nome.Should().Be(cachorro.Nome);
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
                result.Should().BeOfType<NotFoundResult>();
                context.Cachorros.Should().NotContain(c => c.Id == cachorro.Id + 1);
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
                result.Should().BeOfType<CreatedAtActionResult>();
                var model = result.As<CreatedAtActionResult>().Value
                    .Should().BeOfType<Domain.Cachorro>();
                model.Subject.Nome.Should().Be(cachorro.Nome);
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
                var result = await controller.PutCachorroAsync(
                    cachorro.Id,
                    new Domain.Cachorro()
                    {
                        Id = cachorro.Id,
                        Nome = "Sirius v2"
                    });

                // Assert
                result.Should().BeOfType<NoContentResult>();
                //context.Entry(cachorro).State.Should().Equals(EntityState.Modified);
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
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new CachorrosController(context);

                // Act
                var result = await controller.PutCachorroAsync(
                    2,
                    new Domain.Cachorro()
                    {
                        Nome = "Sirius v2"
                    });

                // Assert
                result.Should().BeOfType<BadRequestResult>();
            }
        }

        [Fact]
        [Trait("Delete", "API")]
        public async Task ExcluirCachorroAsync_ReturnsNotFound_WhenCachorroIdDontExists()
        {
            // Arrange
            var cachorroIdInexistente = -1;

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
                var result = await controller.ExcluirCachorroAsync(cachorroIdInexistente);

                // Assert
                result.Should().BeOfType<NotFoundResult>();
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
                result.Should().BeOfType<NoContentResult>();
                context.Cachorros.Should().NotContain(c => c.Id == cachorroId);
            }
        }
    }
}