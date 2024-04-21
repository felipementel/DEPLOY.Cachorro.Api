using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using DEPLOY.Cachorro.Base.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Xunit;

namespace DEPLOY.Cachorro.Api.Tests
{
    [ExcludeFromCodeCoverage]
    public class CachorrosControllerTests : IClassFixture<CachorroDtoFixture>
    {
        private Mock<ICachorroAppServices> _cachorroAppServiceMock;
        private readonly CachorroDtoFixture _cachorroDtoFixture;

        private Controllers.v1.CachorrosController? _cachorroController;

        public CachorrosControllerTests(CachorroDtoFixture cachorroDtoFixture)
        {
            _cachorroDtoFixture = cachorroDtoFixture;
            _cachorroAppServiceMock = new Mock<ICachorroAppServices>();
        }

        [Fact]
        [Trait("Read", "API")]
        public async Task GivenInsertAsync_WhenCachorroIsValid_ThanReturnCreateAtActionResult()
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

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

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
        public async Task GivenInsertAsync_WhenCachorroIsNotValid_ThanReturnUnprocessableEntity()
        {
            // Arrange
            var cachorro = _cachorroDtoFixture.CreateManyCachorroDtoWithERRORName(1)[0];

            // Configurar o Setup para o método InsertAsync
            _cachorroAppServiceMock
                .Setup(x => x.InsertAsync(
                    It.IsAny<CachorroCreateDto>(),
                    CancellationToken.None))
                .ReturnsAsync(cachorro);

            var cachorroCreateDto = new CachorroCreateDto
            (
                Nome: cachorro.Nome,
                Nascimento: cachorro.Nascimento,
                Pelagem: cachorro.Pelagem,
                Peso: cachorro.Peso
            );

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

            // Act
            var result = await _cachorroController.CreateAsync(cachorroCreateDto) as UnprocessableEntityObjectResult;

            // Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status422UnprocessableEntity, result.StatusCode);
            result.Should().BeOfType<UnprocessableEntityObjectResult>();

            var erros = result.Value as List<string>;

            erros.Should().Contain("Nome é obrigatório");
            erros.Should().NotContain("Nome é obrigatório.");

            _cachorroAppServiceMock.Verify(x => x.InsertAsync(It.IsAny<CachorroCreateDto>(),
                CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GivenGetAllAsync_WhenExistemCachorros_ThanDeveRetornarListaCheia()
        {
            //Arrange

            var cachorros = _cachorroDtoFixture.CreateManyCachorroDtoWithTutorDto(2);

            _cachorroAppServiceMock
                .Setup(x => x.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(() => cachorros);

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

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
        public async Task GivenGetAllAsync_WhenCachorrosDontExistis_ThanDeveRetornarListaVazia()
        {
            //Arrange
            var CachorroAppServiceMock = new Mock<ICachorroAppServices>();
            var cachorroController = new Controllers.v1.CachorrosController(CachorroAppServiceMock.Object);

            var cachorros = Enumerable.Empty<CachorroDto>();

            CachorroAppServiceMock
                .Setup(x => x.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(() => cachorros);

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

            // Act
            var result = await cachorroController.ListAllAsync() as NoContentResult;

            //Assert
            Assert.Equal(StatusCodes.Status204NoContent, result?.StatusCode);
            result.Should().BeOfType<NoContentResult>();

            CachorroAppServiceMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GivenGetByIdAsync_WhenCachorroIsIsValdid_ThanShouldReturnNewDog()
        {
            //Arrange
            var cachorro = _cachorroDtoFixture
                .CreateManyCachorroDtoWithTutorDto(1)[0];

            _cachorroAppServiceMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(),
                CancellationToken.None))
                .ReturnsAsync(() => cachorro);

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

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
        public async Task GivenGetByIdAsync_ThanCachorroIdIsNotValid_ShouldReturnNotFound()
        {
            //Arrange
            var cachorro = _cachorroDtoFixture.CreateManyCachorroDtoWithTutorDto(1)[0];

            _cachorroAppServiceMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(),
                CancellationToken.None))
                .ReturnsAsync(() => null);

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

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
        public async Task GivenUpdateAsync_WhenCachorroIsValid_ThanDeveRetornarNoContent()
        {
            //Arrange
            var cachorro = _cachorroDtoFixture.CreateManyCachorroDtoWithTutorDto(1)[0];

            _cachorroAppServiceMock
                .Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CachorroDto>(),
                CancellationToken.None))
                .ReturnsAsync(() => Enumerable.Empty<string>());

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

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
        public async Task GivenDeleteAsync_ThanCachorroIsValid_ThanShouldReturnNoContent()
        {
            //Arrange
            var cachorro = _cachorroDtoFixture.CreateManyCachorroDtoWithTutorDto(1)[0];

            _cachorroAppServiceMock
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>(),
                CancellationToken.None))
                .ReturnsAsync(() => true);

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

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
        public async Task GivenDeleteAsync_WhenCachorroIdIsInvalid_ThanSouldReturnNotFound()
        {
            //Arrange
            var cachorro = _cachorroDtoFixture.CreateManyCachorroDtoWithTutorDto(1)[0];

            _cachorroAppServiceMock
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>(),
                CancellationToken.None))
                .ReturnsAsync(() => false);

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

            // Act
            var result = await _cachorroController.DeleteAsync(cachorro.Id) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            result.Should().BeOfType<NotFoundResult>();

            _cachorroAppServiceMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>(),
                CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GivenListAllCachorrosAdotadosAsync_WhenExistsCachorrosAdotados_ThanDeveRetornarDeCachorrosAdotados()
        {
            // Arrange
            var cachorrosAdotados = _cachorroDtoFixture.CreateManyCachorroDtoWithTutorDto(40);

            _cachorroAppServiceMock.Setup(repo => repo.GetByKeyAsync(
                It.IsAny<Expression<Func<CachorroDto, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(cachorrosAdotados);

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

            // Act
            var controller = await _cachorroController.ListAllCachorrosAdotadosAsync(default);

            // Assert
            Assert.NotNull(controller);

            var result = controller as OkObjectResult;

            result.As<OkObjectResult>().Value
                .Should()
                .BeOfType<List<CachorroDto>>();
        }

        [Fact]
        public async Task GivenListAllCachorrosAdotadosAsync_WhenDontExistsCachorrosAdotados_ThanDeveRetornarDeCachorrosAdotadosVazia()
        {
            // Arrange
            _cachorroAppServiceMock.Setup(repo => repo.GetByKeyAsync(
                It.IsAny<Expression<Func<CachorroDto, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CachorroDto>());

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

            // Act
            var controller = await _cachorroController.ListAllCachorrosAdotadosAsync(default);

            // Assert
            Assert.NotNull(controller);

            var result = controller as NoContentResult;


        }

        [Fact]
        public async Task GivenListAllCachorrosParaAdocaoAsync_WhenCachorrosExistis_ThanDeveRetornarListaVazia()
        {
            // Arrange
            var cachorrosParaAdocao = _cachorroDtoFixture.CreateManyCachorroDtoWithoutTutorDto(40);

            _cachorroAppServiceMock.Setup(repo => repo.GetByKeyAsync(
                It.IsAny<Expression<Func<CachorroDto, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(cachorrosParaAdocao);

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

            // Act
            var controller = await _cachorroController.ListAllCachorrosParaAdocaoAsync(default);

            // Assert
            Assert.NotNull(controller);

            var result = controller as OkObjectResult;

        }

        [Fact]
        public async Task GivenListAllCachorrosParaAdocaoAsync_WhenCachorrosDontExistis_ThanDeveRetornarListaVazia()
        {
            // Arrange
            _cachorroAppServiceMock.Setup(repo => repo.GetByKeyAsync(
                It.IsAny<Expression<Func<CachorroDto, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CachorroDto>());

            _cachorroController = new Controllers.v1.CachorrosController(
                _cachorroAppServiceMock.Object);

            // Act
            var controller = await _cachorroController.ListAllCachorrosParaAdocaoAsync(default);

            // Assert
            Assert.NotNull(controller);

            var result = controller as NoContentResult;
        }
    }
}
