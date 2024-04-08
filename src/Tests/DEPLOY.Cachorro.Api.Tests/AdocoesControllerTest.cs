using DEPLOY.Cachorro.Api.Controllers.v1;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using DEPLOY.Cachorro.Base.Tests;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Validations;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace DEPLOY.Cachorro.Api.Tests
{
    [ExcludeFromCodeCoverage]
    public class AdocoesControllerTest 
        : IClassFixture<CachorroDtoFixture>, IClassFixture<TutorDtoFixture>
    {
        private readonly Mock<IAdocaoAppService> _adocaoAppServiceMock;
        private readonly CachorroDtoFixture _cachorroDtoFixture;
        private readonly TutorDtoFixture _tutorDtoFixture;

        private readonly AdocoesController _adocoesController;

        public AdocoesControllerTest(
            CachorroDtoFixture cachorroDtoFixture,
            TutorDtoFixture tutorDtoFixture)
        {
            _cachorroDtoFixture = cachorroDtoFixture;
            _tutorDtoFixture = tutorDtoFixture;
            _adocaoAppServiceMock = new Mock<IAdocaoAppService>();

            _adocoesController = new AdocoesController(
                _adocaoAppServiceMock.Object);
        }

        [Fact]
        public async Task GivenAdotarAsync_WhenCachorroExists_ThanReturnSuccess()
        {
            // Arrange
            var cachorroAdotado = _cachorroDtoFixture
                .CreateManyCachorroDtoWithoutTutorDto(1)[0];

            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _adocaoAppServiceMock
                .Setup(x => x.AdotarAsync(
                    cachorroAdotado.Id, 
                    tutor.Id,
                    default))
                .ReturnsAsync(Enumerable.Empty<string>());

            // Act
            var result = await _adocoesController.AdotarAsync(
                cachorroAdotado.Id,
                tutor.Id,
                default);

            // Assert
            var item = result as OkResult;
            Assert.NotNull(item);
        }

        [Fact]
        public async Task GivenAdotarAsync_WhenCachorroJaEstaAdotado_ThanReturnSuccess()
        {
            // Arrange
            var cachorroAdotado = _cachorroDtoFixture
                .CreateManyCachorroDtoWithTutorDto(1)[0];

            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _adocaoAppServiceMock
                .Setup(x => x.AdotarAsync(
                    cachorroAdotado.Id,
                    tutor.Id,
                    default))
                .ReturnsAsync(Enumerable.Empty<string>().Append("Cachorro já tem tutor."));

            // Act
            var result = await _adocoesController.AdotarAsync(
                cachorroAdotado.Id,
                tutor.Id,
                default);

            // Assert
            var item = result as UnprocessableEntityObjectResult;
            Assert.NotNull(item);
        }

        [Fact]
        public async Task GivenAdotarAsync_WhenItemExists_ThanReturnSuccess()
        {
            // Arrange
            var cachorroAdotado = _cachorroDtoFixture
                .CreateManyCachorroDtoWithoutTutorDto(1)[0];

            var tutor = _tutorDtoFixture.CreateManyTutorDto(1)[0];

            _adocaoAppServiceMock
                .Setup(x => x.AdotarAsync(
                    cachorroAdotado.Id,
                    tutor.Id,
                    default))
                .ReturnsAsync(Enumerable.Empty<string>());

            // Act
            var result = await _adocoesController.AdotarAsync(
                cachorroAdotado.Id,
                default);

            // Assert
            var item = result as OkResult;
            Assert.NotNull(item);
        }

        [Fact]
        public async Task GivenDevolverAsync_WhenItemExists_ThanReturnSuccess()
        {
            // Arrange
            var cachorroid = Guid.NewGuid();

            _adocaoAppServiceMock
                .Setup(x => x.DevolverAdocaoAsync(cachorroid, default))
                .ReturnsAsync(Enumerable.Empty<string>());

            // Act
            var result = await _adocoesController.DevolverAsync(cachorroid, default);

            // Assert
            var item = result as OkResult;
            Assert.NotNull(item);
        }

        [Fact]
        public async Task GivenDevolverAsync_WhenItemNotExists_ThanReturnUnprocessableEntity()
        {
            // Arrange
            var cachorroid = Guid.NewGuid();

            _adocaoAppServiceMock
                .Setup(x => x.DevolverAdocaoAsync(cachorroid, default))
                .ReturnsAsync(new List<string> { "Cachorro não existe" });

            // Act
            var result = await _adocoesController.DevolverAsync(cachorroid, default);

            // Assert
            var item = result as UnprocessableEntityObjectResult;
            Assert.NotNull(item);
        }

        [Fact]
        public async Task GivenDevolverAsync_WhenCachorroEstaAdotado_ThanReturnUnprocessableEntity()
        {
            // Arrange
            var msgErro = new List<string> { "Cachorro já esta adotado" };

            var cachorroAdotado = _cachorroDtoFixture.CreateManyCachorroDtoWithERRORName(1)[0];

            _adocaoAppServiceMock
                .Setup(x => x.DevolverAdocaoAsync(cachorroAdotado.Id, default))
                .ReturnsAsync(msgErro);

            // Act
            var result = await _adocoesController.DevolverAsync(cachorroAdotado.Id, default) as UnprocessableEntityObjectResult;

            var cachorroValidado = result?.Value as IEnumerable<string>;

            cachorroValidado.Should().BeOfType<List<string>>();

            // Assert
            Assert.NotNull(result);
            result.Equals(StatusCodes.Status422UnprocessableEntity);

            //result.Should().Subject.Should().BeOfType<TutorDto>();

            //var model = result
            //    .As<UnprocessableEntityObjectResult>()
            //    .Value
            //    .Should()
            //    .BeOfType<IEnumerable<string>>();

            //model.Subject.Should().OnlyContain(x => x.Equals("Cachorro já esta adotado."));            

            // test the message
            //Assert.Equal("Cachorro já esta adotado.", cachorroValidator[0].ErrorMessage);
        }
    }
}