using DEPLOY.Cachorro.Api.Controllers.v1;
using DEPLOY.Cachorro.Repository;
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
                var okResult = Assert.IsType<OkObjectResult>(result);
                var model = Assert.IsType<Domain.Tutor>(okResult.Value);
                Assert.Equal(tutor.Id, model.Id);
                Assert.Equal(tutor.Nome, model.Nome);
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
                Assert.IsType<NotFoundResult>(result);

                using (var contextAfterDelete = new CachorroDbContext(options))
                {
                    var deletedCachorro = await contextAfterDelete.Cachorros.FindAsync(tutor.Id + 1);
                    Assert.Null(deletedCachorro);
                }
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
                var okResult = Assert.IsType<CreatedAtActionResult>(result);
                var model = Assert.IsType<Domain.Tutor>(okResult.Value);
                Assert.Equal(tutor.Nome, model.Nome);
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
                Assert.IsType<NoContentResult>(result);
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
                Assert.IsType<BadRequestResult>(result);
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
                Assert.IsType<NotFoundResult>(result);

                using (var contextAfterDelete = new CachorroDbContext(options))
                {
                    var deletedCachorro = await contextAfterDelete.Tutores.FindAsync(tutor.Id);
                    Assert.NotNull(deletedCachorro);
                }
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
                Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);

                using (var contextAfterDelete = new CachorroDbContext(options))
                {
                    var deletedCachorro = await contextAfterDelete.Cachorros.FindAsync(tutor.Id);
                    Assert.Null(deletedCachorro);
                }
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