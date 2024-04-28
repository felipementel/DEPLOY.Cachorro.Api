using DEPLOY.Cachorro.Api.Controllers.v1;
using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using DEPLOY.Cachorro.Base.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace DEPLOY.Cachorro.Api.Tests
{
    [ExcludeFromCodeCoverage]
    public class TutoresControllerTest : IClassFixture<TutorDtoFixture>
    {
        private readonly Mock<ITutorAppServices> _tutorAppServiceMock;
        private readonly TutorDtoFixture _tutorDtoFixture;

        private Controllers.v1.TutoresController? _tutoresController;

        public TutoresControllerTest(TutorDtoFixture tutorDtoFixture)
        {
            _tutorDtoFixture = tutorDtoFixture;
            _tutorAppServiceMock = new Mock<ITutorAppServices>();
        }

        [Fact]
        public async Task GivenListAllAsync_WhenExistsTutores_ThanReturnListWithTutores()
        {
            // Arrange
            var tutor = _tutorDtoFixture.CreateManyTutorDto(3);

            _tutorAppServiceMock
                .Setup(t => t.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(tutor);

            _tutoresController = new Controllers.v1.TutoresController(_tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController.ListAllAsync(CancellationToken.None) as OkObjectResult;

            //Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

            result.Should().BeOfType<OkObjectResult>();

            var model = result.As<OkObjectResult>().Value
                .Should()
                .BeOfType<List<TutorDto>>();

            model.Subject.Count.Should().Be(3);

            _tutorAppServiceMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GivenListAllAsync_WhenNotExistsTutores_ThanReturnEmptyList()
        {
            // Arrange
            var tutor = Enumerable.Empty<TutorDto>();

            _tutorAppServiceMock
                .Setup(t => t.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(tutor);

            _tutoresController = new Controllers.v1.TutoresController(_tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController.ListAllAsync(CancellationToken.None);

            //Assert
            result.Should().BeOfType<NoContentResult>();

            _tutorAppServiceMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GivenGetByIdAsync_WhenIdIsValid_ThanSuccess()
        {
            // Arrange
            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _tutorAppServiceMock
                .Setup(t => t.GetByIdAsync(tutor.Id, CancellationToken.None))
                .ReturnsAsync(tutor);

            _tutoresController = new Controllers.v1.TutoresController(_tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController
                .GetByIdAsync(
                    tutor.Id,
                    CancellationToken.None)
                as OkObjectResult;

            // Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

            result.Should().BeOfType<OkObjectResult>();

            _tutorAppServiceMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task GivenGetByIdAsync_WhenTutorIdIsNotValid_ThanNorFoundResult()
        {
            // Arrange
            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _tutorAppServiceMock
                .Setup(t => t.GetByIdAsync(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(() => null);

            _tutoresController = new Controllers.v1.TutoresController(_tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController.GetByIdAsync(tutor.Id, CancellationToken.None) as NotFoundResult;

            // Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);

            result.Should().BeOfType<NotFoundResult>();

            _tutorAppServiceMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Never);
            _tutorAppServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<long>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GivenTutorCreateAsync_WhenTutorIsValid_ThanReturnCreatedAction201()
        {
            // Arrange
            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _tutorAppServiceMock
                .Setup(x => x.InsertAsync(It.IsAny<TutorDto>(),
                CancellationToken.None))
                .ReturnsAsync(tutor);

            _tutoresController = new Controllers.v1.TutoresController(_tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController.CreateAsync(tutor) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            result.Should().BeOfType<CreatedAtActionResult>();

            Assert.Equal("GetById", result.ActionName);

            var routeValues = result.RouteValues;
            Assert.NotNull(routeValues);
            Assert.True(routeValues.ContainsKey("id"));
            Assert.True(routeValues.ContainsKey("version"));

            Assert.Equal("1.0", routeValues["version"]);

            Assert.Equal(tutor, result.Value);

            _tutorAppServiceMock.Verify(x => x.InsertAsync(It.IsAny<TutorDto>(),
                CancellationToken.None), Times.Once);

            var model = result.As<CreatedAtActionResult>().Value
                .Should().BeOfType<TutorDto>();

            model.Subject.Nome.Should().Be(tutor.Nome);
            model.Subject.Erros.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GivenTutorCreateAsync_WhenTutorIsNotValid_ThanReturnUnprocessableEntity()
        {
            // Arrange
            var tutor = _tutorDtoFixture.CreateManyTutorDtoWithNameError(1)[0];

            _tutorAppServiceMock
                .Setup(x => x.InsertAsync(It.IsAny<TutorDto>(),
                CancellationToken.None))
                .ReturnsAsync(tutor);

            _tutoresController = new Controllers.v1.TutoresController(_tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController.CreateAsync(tutor) as UnprocessableEntityObjectResult;

            // Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status422UnprocessableEntity, result.StatusCode);
            result.Should().BeOfType<UnprocessableEntityObjectResult>();

            Assert.Equal(tutor.Erros, result.Value);

            _tutorAppServiceMock.Verify(x => x.InsertAsync(It.IsAny<TutorDto>(),
                CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GivenUpdateAsync_WhenTutorIsValid_ThanReturnNoContent()
        {
            // Arrange
            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _tutorAppServiceMock
                .Setup(t => t.UpdateAsync(tutor.Id, tutor, CancellationToken.None))
                .ReturnsAsync(Enumerable.Empty<string>());

            _tutoresController = new Controllers.v1.TutoresController(_tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController.UpdateAsync(tutor.Id, tutor, CancellationToken.None) as NoContentResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result?.StatusCode);

            result.Should().BeOfType<NoContentResult>();

            _tutorAppServiceMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Never);
            _tutorAppServiceMock.Verify(x => x.UpdateAsync(tutor.Id, tutor, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GivenUpdateAsync_WhenIdIsNotEqual_ThanError()
        {
            // Arrange
            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            //_tutorAppServiceMock
            //    .Setup(t => t.UpdateAsync(tutor.Id, tutor, CancellationToken.None))
            //    .ReturnsAsync(Enumerable.Empty<string>().Append("Tutor não existe"));

            _tutoresController = new TutoresController(
                               _tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController.UpdateAsync(It.IsAny<long>(), tutor, CancellationToken.None);

            // Assert
            result.Should().BeOfType<UnprocessableEntityResult>();          

            _tutorAppServiceMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Never);
            _tutorAppServiceMock.Verify(x => x.UpdateAsync(tutor.Id, tutor, CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task GivenUpdateAsync_WhenTutorDontExistis_ThanReturnUnprocessableEntity()
        {
            // Arrange
            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _tutorAppServiceMock
                .Setup(t => t.UpdateAsync(It.IsAny<long>(), tutor, CancellationToken.None))
                .ReturnsAsync(Enumerable.Empty<string>().Append("Tutor não encontrado"));

            _tutoresController = new Controllers.v1.TutoresController(_tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController.UpdateAsync(tutor.Id, tutor, CancellationToken.None) as UnprocessableEntityObjectResult;

            // Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status422UnprocessableEntity, result.StatusCode);

            result.Should().BeOfType<UnprocessableEntityObjectResult>();

            _tutorAppServiceMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Never);
            _tutorAppServiceMock.Verify(x => x.UpdateAsync(tutor.Id, tutor, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GivenUpdateAsync_WhenIdIsEqualButTutorDontExists_ThanError()
        {
            // Arrange
            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _tutorAppServiceMock
                .Setup(t => t.UpdateAsync(tutor.Id, tutor, CancellationToken.None))
                .ReturnsAsync(Enumerable.Empty<string>().Append("Tutor não existe"));

            var tutor2 = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _tutoresController = new TutoresController(
                               _tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController.UpdateAsync(tutor.Id, tutor, CancellationToken.None) as UnprocessableEntityObjectResult;

            // Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status422UnprocessableEntity, result.StatusCode);

            result.Should().BeOfType<UnprocessableEntityObjectResult>();

            _tutorAppServiceMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Never);
            _tutorAppServiceMock.Verify(x => x.UpdateAsync(tutor.Id, tutor, CancellationToken.None), Times.Once);
            _tutorAppServiceMock.Verify(x => x.UpdateAsync(tutor2.Id, tutor2, CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task GivenUpdateAsync_WhenIdIsDifferents_ThanError()
        {
            // Arrange
            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _tutorAppServiceMock
                .Setup(t => t.UpdateAsync(It.IsAny<long>(), tutor, CancellationToken.None))
                .ReturnsAsync(Enumerable.Empty<string>().Append("Tutor não existe"));

            var tutor2 = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _tutoresController = new TutoresController(
                               _tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController.UpdateAsync(tutor.Id, tutor, CancellationToken.None) as UnprocessableEntityObjectResult;

            // Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status422UnprocessableEntity, result.StatusCode);

            result.Should().BeOfType<UnprocessableEntityObjectResult>();

            _tutorAppServiceMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Never);
            _tutorAppServiceMock.Verify(x => x.UpdateAsync(tutor.Id, tutor, CancellationToken.None), Times.Once);
            _tutorAppServiceMock.Verify(x => x.UpdateAsync(tutor2.Id, tutor2, CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task GivenDeleteAsync_WhenTutorExists_ThanSuccess()
        {
            //Arrange
            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _tutorAppServiceMock
                .Setup(x => x.DeleteAsync(It.IsAny<long>(),
                CancellationToken.None))
                .ReturnsAsync(() => true);

            _tutoresController = new Controllers.v1.TutoresController(_tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController.DeleteAsync(tutor.Id) as NoContentResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
            result.Should().BeOfType<NoContentResult>();

            _tutorAppServiceMock.Verify(x => x.DeleteAsync(It.IsAny<long>(),
                CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GivenDeleteAsync_WhenTutorDontExists_ThanReturnNotFound()
        {
            //Arrange
            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _tutorAppServiceMock
                .Setup(x => x.DeleteAsync(It.IsAny<long>(),
                CancellationToken.None))
                .ReturnsAsync(() => false);

            _tutoresController = new Controllers.v1.TutoresController(_tutorAppServiceMock.Object);

            // Act
            var result = await _tutoresController.DeleteAsync(tutor.Id) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            result.Should().BeOfType<NotFoundResult>();

            _tutorAppServiceMock.Verify(x => x.DeleteAsync(It.IsAny<long>(),
                CancellationToken.None), Times.Once);
        }
    }
}