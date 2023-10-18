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
    public class TutoresControllerTest
    {
        [Fact]
        [Trait("List", "API")]
        public async Task ListarAsync_ReturnsOk_WhenTutoresIsFound()
        {
            // Arrange
            var tutor1 = new Domain.Tutor { Nome = "Eu cuido da silva" };
            var tutor2 = new Domain.Tutor { Nome = "Eu cuido dos santos" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Tutores.Add(tutor1);
                context.Tutores.Add(tutor2);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new TutoresController(context);

                // Act
                var result = await controller.ListarAsync();

                // Assert
                result.Should().BeOfType<OkObjectResult>();
                result.As<OkObjectResult>().Value.Should().BeOfType<List<Domain.Tutor>>();
            }
        }

        [Fact]
        [Trait("List", "API")]
        public async Task ListarAsync_ReturnsOk_WhenTutoresNotFound()
        {
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
                var controller = new TutoresController(context);

                // Act
                var result = await controller.ListarAsync();

                // Assert
                result.Should().BeOfType<OkObjectResult>();
                result.As<OkObjectResult>().Value.Should().BeOfType<List<Domain.Tutor>>();
                result.As<List<Domain.Tutor>>().Should().BeNull();
            }
        }

        [Fact]
        [Trait("Read", "API")]
        public async Task ObterPorIdAsync_ReturnsOk_WhenTutorIsFound()
        {
            // Arrange
            var tutor = new Domain.Tutor { Nome = "Eu cuido da silva" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Tutores.Add(tutor);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new TutoresController(context);

                // Act
                var result = await controller.ObterPorIdAsync(tutor.Id);

                // Assert
                result.Should().BeOfType<OkObjectResult>();
                var model = result.As<OkObjectResult>().Value.Should().BeOfType<Domain.Tutor>();
                model.Subject.Id.Should().Be(tutor.Id);
                model.Subject.Nome.Should().Be(tutor.Nome);
            }
        }

        [Fact]
        [Trait("Read", "API")]
        public async Task ObterPorIdAsync_ReturnsNotFound_WhenTutorNotFound()
        {
            // Arrange
            var tutor = new Domain.Tutor { Nome = "Eu cuido da silva" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Tutores.Add(tutor);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new TutoresController(context);

                // Act
                var result = await controller.ObterPorIdAsync(tutor.Id + 1);

                // Assert
                result.Should().BeOfType<NotFoundResult>();
                context.Tutores.Should().NotContain(t => t.Id == tutor.Id + 1);
            }
        }

        [Fact]
        [Trait("Create", "API")]
        public async Task CadastrarTutotAsync_ReturnsCreated_WhenTutorIsValid()
        {
            // Arrange
            var tutor = new Domain.Tutor { Nome = "Eu cuido da silva" };

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
                var controller = new TutoresController(context);

                // Act
                var result = await controller.CadastrarTutorAsync(tutor);

                // Assert
                result.Should().BeOfType<CreatedAtActionResult>();
                var model = result.As<CreatedAtActionResult>().Value.Should().BeOfType<Domain.Tutor>();
                model.Subject.Nome.Should().Be(tutor.Nome);
            }
        }

        [Fact]
        [Trait("Update", "API")]
        public async Task PutTutotAsync_ReturnsNoContent_WhenTutorIsValid()
        {
            // Arrange
            var tutor = new Domain.Tutor { Nome = "Eu cuido da silva" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Tutores.Add(tutor);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new TutoresController(context);

                // Act
                var result = await controller.PutTutorAsync(tutor.Id, new Domain.Tutor() { Id = tutor.Id, Nome = "Sirius v2" });

                // Assert
                result.Should().BeOfType<NoContentResult>();
            }
        }

        [Fact]
        [Trait("Update", "API")]
        public async Task PutTutorAsync_ReturnsBadRequest_WhenTutorIsInvalid()
        {
            // Arrange
            var tutor = new Domain.Tutor { Nome = "Eu cuido da silva" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
               .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Tutores.Add(tutor);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new TutoresController(context);

                // Act
                var result = await controller.PutTutorAsync(2, new Domain.Tutor() { Nome = "Nome Atualizado" });

                // Assert
                result.Should().BeOfType<BadRequestResult>();
            }
        }

        [Fact]
        [Trait("Delete", "API")]
        public async Task ExcluirTutorAsync_ReturnsNotFound_WhenTutorIdDontExists()
        {
            // Arrange
            var TutorId = 2;
            var tutor = new Domain.Tutor { Id = TutorId, Nome = "Eu cuido da silva" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Tutores.Add(tutor);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new TutoresController(context);

                // Act
                var result = await controller.ExcluirTutorAsync(tutor.Id + 1);

                // Assert
                result.Should().BeOfType<NotFoundResult>();
                context.Tutores.Should().NotContain(t => t.Id == tutor.Id + 1);
            }
        }

        [Fact]
        [Trait("Delete", "API")]
        public async Task ExcluirTutorAsync_ReturnsNoContent_WhenTutorIdIsValid()
        {
            // Arrange
            var tutor = new Domain.Tutor { Nome = "Eu cuido da silva" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Tutores.Add(tutor);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new TutoresController(context);

                // Act
                var result = await controller.ExcluirTutorAsync(tutor.Id);

                // Assert
                result.Should().BeOfType<NoContentResult>();
                context.Tutores.Should().NotContain(t => t.Id == tutor.Id);
            }
        }

        [Fact]
        [Trait("Delete", "API")]
        public async Task ExcluirTutorAsync_ReturnsNoContent_WhenTutorIsDeleted()
        {
            // Arrange
            var cachorroId = 1;
            var tutor = new Domain.Tutor { Id = cachorroId, Nome = "Eu cuido da silva" };

            var options = new DbContextOptionsBuilder<CachorroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new CachorroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Tutores.Add(tutor);
                context.SaveChanges();
            }

            using (var context = new CachorroDbContext(options))
            {
                var controller = new TutoresController(context);

                // Act
                var result = await controller.ExcluirTutorAsync(cachorroId);

                // Assert
                result.Should().BeOfType<NoContentResult>();
                context.Cachorros.Should().NotContain(c => c.Id == cachorroId);
            }
        }
    }
}