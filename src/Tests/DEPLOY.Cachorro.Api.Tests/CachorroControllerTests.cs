using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using DEPLOY.Cachorro.Base.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DEPLOY.Cachorro.Api.Tests
{
    public class CachorroControllerTests : IClassFixture<CachorroDtoFixture>
    {
        private readonly CachorroDtoFixture _cachorroDtoFixture;
        private readonly Mock<ICachorroAppServices> _cachorroAppServiceMock;
        private readonly Controllers.v1.CachorrosController _cachorroController;

        public CachorroControllerTests()
        {
            _cachorroDtoFixture = new();

            _cachorroAppServiceMock = new Mock<ICachorroAppServices>();
            _cachorroController = new Controllers.v1.CachorrosController(_cachorroAppServiceMock.Object);
        }

        [Fact]
        [Trait("Read", "API")]
        public async Task CadastrarCachorroAsync_DeveRetornarCreatedResult_ComIdEVersionQuandoSucesso()
        {
            // Arrange
            var cachorro = _cachorroDtoFixture.CreateManyCachorroDtoWithTutorDto(1)[0];

            // Configurar o Setup para o método InsertAsync
            _cachorroAppServiceMock
                .Setup(x => x.InsertAsync(It.IsAny<CachorroCreateDto>(),
                CancellationToken.None))
                .ReturnsAsync(cachorro);

            var cachorroCreateDto = new CachorroCreateDto
            (
                Nome: cachorro.Nome,
                Nascimento: cachorro.Nascimento,
                Pelagem: cachorro.Pelagem,
                Peso: cachorro.Peso
            );

            // Act
            var result = await _cachorroController.CreateAsync(cachorroCreateDto) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            result.Should().BeOfType<CreatedAtActionResult>();

            Assert.Equal("GetById", result.ActionName);

            var routeValues = result.RouteValues;
            Assert.NotNull(routeValues);
            Assert.True(routeValues.ContainsKey("id"));
            Assert.True(routeValues.ContainsKey("version"));

            //Assert.Equal(cachorro.Id, routeValues["id"]);
            Assert.Equal("1.0", routeValues["version"]);

            Assert.Equal(cachorro, result.Value);

            _cachorroAppServiceMock.Verify(x => x.InsertAsync(It.IsAny<CachorroCreateDto>(),
                CancellationToken.None), Times.Once);

            var model = result.As<CreatedAtActionResult>().Value
                .Should().BeOfType<CachorroDto>();

            model.Subject.Nome.Should().Be(cachorro.Nome);
        }

        [Fact]
        public async Task ObterTodosCachorroAsync_DeveRetornarListaCheia()
        {
            //Arrange

            var cachorros = _cachorroDtoFixture.CreateManyCachorroDtoWithTutorDto(2);

            _cachorroAppServiceMock
                .Setup(x => x.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(() => cachorros);

            // Act
            var result = await _cachorroController.ListAllAsync() as OkObjectResult;

            //Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

            result.Should().BeOfType<OkObjectResult>();

            var model = result.As<OkObjectResult>().Value
                .Should()
                .BeOfType<List<CachorroDto>>();

            model.Subject.Count.Should().Be(2);

            _cachorroAppServiceMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task ObterTodosCachorroAsync_DeveRetornarListaVazia()
        {
            //Arrange
            var CachorroAppServiceMock = new Mock<ICachorroAppServices>();
            var cachorroController = new Controllers.v1.CachorrosController(CachorroAppServiceMock.Object);

            var cachorros = Enumerable.Empty<CachorroDto>();

            CachorroAppServiceMock
                .Setup(x => x.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(() => cachorros);

            // Act
            var result = await cachorroController.ListAllAsync() as NoContentResult;

            //Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
            var model = result.Should().BeOfType<NoContentResult>();

            CachorroAppServiceMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task ObterCachorroPorId_DeveRetornarCachorro()
        {
            //Arrange
            var cachorro = _cachorroDtoFixture
                .CreateManyCachorroDtoWithTutorDto(1)[0];

            _cachorroAppServiceMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(),
                CancellationToken.None))
                .ReturnsAsync(() => cachorro);

            // Act
            var result = await _cachorroController.GetByIdAsync(cachorro.Id) as OkObjectResult;

            //Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

            result.Should().BeOfType<OkObjectResult>();

            var model = result
                .As<OkObjectResult>().Value
                .Should()
                .BeOfType<CachorroDto>();

            model.Subject.Nome
                .Should()
                .Be(cachorro.Nome);

            _cachorroAppServiceMock.Verify(x => x.GetByIdAsync(
                    It.IsAny<Guid>(),
                    CancellationToken.None),
                Times.Once);
        }

        [Fact]
        public async Task ObterCachorroPorId_DeveRetornarNotFound()
        {
            //Arrange
            var cachorro = _cachorroDtoFixture.CreateManyCachorroDtoWithTutorDto(1)[0];

            _cachorroAppServiceMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(),
                CancellationToken.None))
                .ReturnsAsync(() => null);

            // Act
            var result = await _cachorroController.GetByIdAsync(cachorro.Id) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            result.Should().BeOfType<NotFoundResult>();

            _cachorroAppServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(),
                CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task AtualizarCachorro_DeveRetornarNoContent()
        {
            //Arrange
            var cachorro = _cachorroDtoFixture.CreateManyCachorroDtoWithTutorDto(1)[0];

            _cachorroAppServiceMock
                .Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CachorroDto>(),
                CancellationToken.None))
                .ReturnsAsync(() => true);

            // Act
            var result = await _cachorroController.UpdateAsync(cachorro.Id, cachorro) as NoContentResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
            result.Should().BeOfType<NoContentResult>();

            _cachorroAppServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CachorroDto>(),
                CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeletarCachorro_DeveRetornarNoContent()
        {
            //Arrange
            var cachorro = _cachorroDtoFixture.CreateManyCachorroDtoWithTutorDto(1)[0];

            _cachorroAppServiceMock
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>(),
                CancellationToken.None))
                .ReturnsAsync(() => true);

            // Act
            var result = await _cachorroController.DeleteAsync(cachorro.Id) as NoContentResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
            result.Should().BeOfType<NoContentResult>();

            _cachorroAppServiceMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>(),
                CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeletarCachorro_DeveRetornarNotFound()
        {
            //Arrange
            var cachorro = _cachorroDtoFixture.CreateManyCachorroDtoWithTutorDto(1)[0];

            _cachorroAppServiceMock
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>(),
                CancellationToken.None))
                .ReturnsAsync(() => false);

            // Act
            var result = await _cachorroController.DeleteAsync(cachorro.Id) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            result.Should().BeOfType<NotFoundResult>();

            _cachorroAppServiceMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>(),
                CancellationToken.None), Times.Once);
        }
    }
}
