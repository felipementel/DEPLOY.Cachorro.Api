using Asp.Versioning;
using Bogus;
using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Api.Tests
{
    [ExcludeFromCodeCoverage]
    public class CachorrosControllerTestOLD
    {
        
    }
    //[Fact]
    //[Trait("Read", "API")]
    //public async Task ListarAsync_ReturnOk()
    //{
    //    // Arrange
    //    var cachorro = new Domain.Cachorro { Nome = "Sirius" };

    //    var options = new DbContextOptionsBuilder<CachorroDbContext>()
    //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    //        .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
    //        .Options;

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        context.Database.EnsureDeleted();
    //        context.Database.EnsureCreated();

    //        context.Cachorros.Add(cachorro);
    //        context.SaveChanges();
    //    }

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        var controller = new CachorrosController(context);

    //        // Act
    //        var result = await controller.ListarAsync();

    //        // Assert
    //        result.Should().BeOfType<OkObjectResult>();
    //        result.As<OkObjectResult>().Value
    //            .Should().BeOfType<List<Domain.Aggregate.Cachorro.Entities.Cachorro>>();
    //    }
    //}

    //[Fact]
    //[Trait("Read", "API")]
    //public async Task ObterPorIdAsync_ReturnsOk_WhenCachorroIsFound()
    //{
    //    // Arrange
    //    var cachorro = new Domain.Cachorro { Nome = "Sirius" };

    //    var options = new DbContextOptionsBuilder<CachorroDbContext>()
    //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    //        .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
    //        .Options;

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        context.Database.EnsureDeleted();
    //        context.Database.EnsureCreated();

    //        context.Cachorros.Add(cachorro);
    //        context.SaveChanges();
    //    }

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        var controller = new CachorrosController(context);

    //        // Act
    //        var result = await controller.ObterPorIdAsync(cachorro.Id);

    //        // Assert
    //        result.Should().BeOfType<OkObjectResult>();
    //        var model = result.As<OkObjectResult>().Value
    //            .Should().BeOfType<Domain.Aggregate.Cachorro.Entities.Cachorro>();
    //        model.Subject.Id.Should().Be(cachorro.Id);
    //        model.Subject.Nome.Should().Be(cachorro.Nome);
    //    }
    //}

    //[Fact]
    //[Trait("Read", "API")]
    //public async Task ObterPorIdAsync_ReturnsNotFound_WhenCachorroNotFound()
    //{
    //    // Arrange
    //    var cachorro = new Domain.Cachorro { Nome = "Sirius" };

    //    var options = new DbContextOptionsBuilder<CachorroDbContext>()
    //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    //        .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
    //        .Options;

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        context.Database.EnsureDeleted();
    //        context.Database.EnsureCreated();

    //        context.Cachorros.Add(cachorro);
    //        context.SaveChanges();
    //    }

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        var controller = new CachorrosController(context);

    //        // Act
    //        var result = await controller.ObterPorIdAsync(Guid.NewGuid());

    //        // Assert
    //        result.Should().BeOfType<NotFoundResult>();
    //        context.Cachorros.Should().NotContain(c => c.Id == Guid.NewGuid());
    //    }
    //}

    //[Fact]
    //[Trait("Create", "API")]
    //public async Task CadastrarCachorroAsync_ReturnsCreated_WhenCachorroIsValid()
    //{
    //    // Arrange
    //    var cachorro = new Domain.Cachorro { Nome = "Sirius" };

    //    var options = new DbContextOptionsBuilder<CachorroDbContext>()
    //       .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    //       .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
    //       .Options;

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        context.Database.EnsureDeleted();
    //        context.Database.EnsureCreated();
    //    }

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        var controller = new CachorrosController(context);

    //        // Act
    //        var result = await controller.CadastrarCachorroAsync(cachorro);

    //        // Assert
    //        result.Should().BeOfType<CreatedAtActionResult>();
    //        var model = result.As<CreatedAtActionResult>().Value
    //            .Should().BeOfType<Domain.Aggregate.Cachorro.Entities.Cachorro>();
    //        model.Subject.Nome.Should().Be(cachorro.Nome);
    //    }
    //}

    //[Fact]
    //[Trait("Update", "API")]
    //public async Task PutCachorroAsync_ReturnsNoContent_WhenCachorroIsValid()
    //{
    //    // Arrange
    //    var cachorro = new Domain.Cachorro { Nome = "Sirius" };

    //    var options = new DbContextOptionsBuilder<CachorroDbContext>()
    //       .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    //       .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
    //       .Options;

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        context.Database.EnsureDeleted();
    //        context.Database.EnsureCreated();

    //        context.Cachorros.Add(cachorro);
    //        context.SaveChanges();
    //    }

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        var controller = new CachorrosController(context);

    //        // Act
    //        var result = await controller.PutCachorroAsync(
    //            cachorro.Id,
    //            new Domain.Cachorro()
    //            {
    //                Id = cachorro.Id,
    //                Nome = "Sirius v2"
    //            });

    //        // Assert
    //        result.Should().BeOfType<NoContentResult>();
    //        //context.Entry(cachorro).State.Should().Equals(EntityState.Modified);
    //    }
    //}

    //[Fact]
    //[Trait("Update", "API")]
    //public async Task PutCachorroAsync_ReturnsBadRequest_WhenCachorroIsInvalid()
    //{
    //    // Arrange
    //    var cachorro = new Domain.Cachorro { Nome = "Sirius" };

    //    var options = new DbContextOptionsBuilder<CachorroDbContext>()
    //       .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    //       .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
    //       .Options;

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        context.Database.EnsureDeleted();
    //        context.Database.EnsureCreated();

    //        context.Cachorros.Add(cachorro);
    //        context.SaveChanges();
    //    }

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        var controller = new CachorrosController(context);

    //        // Act
    //        var result = await controller.PutCachorroAsync(
    //            Guid.NewGuid(),
    //            new Domain.Cachorro()
    //            {
    //                Nome = "Sirius v2"
    //            });

    //        // Assert
    //        result.Should().BeOfType<BadRequestResult>();
    //    }
    //}

    //[Fact]
    //[Trait("Delete", "API")]
    //public async Task ExcluirCachorroAsync_ReturnsNotFound_WhenCachorroIdDontExists()
    //{
    //    // Arrange
    //    var cachorroIdInexistente = Guid.NewGuid();

    //    var options = new DbContextOptionsBuilder<CachorroDbContext>()
    //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    //        .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
    //        .Options;

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        context.Database.EnsureDeleted();
    //        context.Database.EnsureCreated();
    //    }

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        var controller = new CachorrosController(context);

    //        // Act
    //        var result = await controller.ExcluirCachorroAsync(cachorroIdInexistente);

    //        // Assert
    //        result.Should().BeOfType<NotFoundResult>();
    //    }
    //}

    //[Fact]
    //[Trait("Delete", "API")]
    //public async Task ExcluirCachorroAsync_ReturnsNoContent_WhenCachorroIsDeleted()
    //{
    //    // Arrange
    //    var cachorroId = Guid.NewGuid();
    //    var cachorro = new Domain.Cachorro { Id = cachorroId, Nome = "Sirius" };

    //    var options = new DbContextOptionsBuilder<CachorroDbContext>()
    //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    //        .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
    //        .Options;

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        context.Database.EnsureDeleted();
    //        context.Database.EnsureCreated();

    //        context.Cachorros.Add(cachorro);
    //        context.SaveChanges();
    //    }

    //    using (var context = new CachorroDbContext(options))
    //    {
    //        var controller = new CachorrosController(context);

    //        // Act
    //        var result = await controller.ExcluirCachorroAsync(cachorroId);

    //        // Assert
    //        result.Should().BeOfType<NoContentResult>();
    //        context.Cachorros.Should().NotContain(c => c.Id == cachorroId);
    //    }
    //}


    //[Fact]
    //public async Task AdotarCachorro_DeveRetornarNoContent()
    //{
    //    //Arrange
    //    var mockCachorroAppService = new Mock<ICachorroAppServices>();
    //    var cachorroController = new Controllers.v1.CachorrosController(mockCachorroAppService.Object);

    //    var cachorro = new Faker<Domain.Aggregates.Cachorro.Entities.Cachorro>()
    //        .RuleFor(x => x.Id, f => f.Random.Guid())
    //        .RuleFor(x => x.Nome, f => f.Person.FirstName)
    //        .RuleFor(x => x.Pelagem, f => f.PickRandom<Domain.Aggregates.Cachorro.ValueObject.PELAGEM>())
    //        .RuleFor(x => x.Peso, f => f.Random.Float(1, 100))
    //        .RuleFor(x => x.Nascimento, f => f.Date.Past())
    //        .Generate();

    //    var items = mockCachorroAppService.Setup(x => x.AdotarCachorro(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(() => Task.CompletedTask);

    //    // Act
    //    var result = await cachorroController.AdotarCachorro(cachorro.Id, cachorro.Id) as NoContentResult;

    //    //Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
    //    result.Should().BeOfType<NoContentResult>();

    //    mockCachorroAppService.Verify(x => x.AdotarCachorro(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
    //}

}