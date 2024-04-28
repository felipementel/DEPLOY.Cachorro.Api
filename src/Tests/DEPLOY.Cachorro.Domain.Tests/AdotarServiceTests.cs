using DEPLOY.Cachorro.Base.Tests;
using DEPLOY.Cachorro.Domain.Aggregates.Adotar.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Repositories;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Entities;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Repositories;
using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;
using FluentAssertions;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain.Tests
{
    [ExcludeFromCodeCoverage]
    public class AdotarServiceTests : IClassFixture<CachorroFixture>, IClassFixture<TutorFixture>
    {
        private readonly CachorroFixture _cachorroFixture;
        private readonly TutorFixture _tutorFixture;

        private readonly Mock<ICachorroRepository> _cachorroRepositoryMock;
        private readonly Mock<ITutorRepository> _tutorRepositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;

        public AdotarServiceTests(
            CachorroFixture cachorroFixture,
            TutorFixture tutorFixture)
        {
            _cachorroFixture = cachorroFixture;
            _tutorFixture = tutorFixture;

            _cachorroRepositoryMock = new Mock<ICachorroRepository>();
            _tutorRepositoryMock = new Mock<ITutorRepository>();
            _uowMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task GivenAdotarAsync_WhenCachorroAndTutorExiste_ThanDeveRetornarSucesso()
        {
            // Arrange
            var adocaoService = new AdocaoService(
                _uowMock.Object,
                _cachorroRepositoryMock.Object,
                _tutorRepositoryMock.Object);

            var cachorro = _cachorroFixture.CreateManyCachorroWithoutTutor(1)[0];
            var tutor = _tutorFixture.CreateManyTutores(1)[0];

            _cachorroRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(cachorro);

            _tutorRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(tutor);

            // Act
            var result = await adocaoService.AdotarAsync(It.IsAny<Guid>(), It.IsAny<int>());

            // Assert
            result.Should().BeOfType<List<string>>();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GivenAdotarAsync_WhenCachorroNaoEncontrado_ThanDeveRetornarErro()
        {
            // Arrange
            var adocaoService = new AdocaoService(
                _uowMock.Object,
                _cachorroRepositoryMock.Object,
                _tutorRepositoryMock.Object);

            _cachorroRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(null as Domain.Aggregates.Cachorro.Entities.Cachorro);

            // Act
            var result = await adocaoService.AdotarAsync(Guid.NewGuid(), 1);

            // Assert
            result.Should().BeOfType<List<string>>();

            Assert.Contains("Cachorro não encontrado", result);
        }

        [Fact]
        public async Task GivenAdotarAsync_WhenCachorroJaAdotado_ThanDeveRetornarErro()
        {
            // Arrange
            var cachorroAdotado = _cachorroFixture.CreateManyCachorroWithTutor(1)[0];

            var adocaoService = new AdocaoService(
                _uowMock.Object,
                _cachorroRepositoryMock.Object,
                _tutorRepositoryMock.Object);

            _tutorRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(cachorroAdotado.Tutor);

            _cachorroRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(cachorroAdotado);

            // Act
            var result = await adocaoService.AdotarAsync(Guid.NewGuid(), 1);

            // Assert
            result.Should().BeOfType<List<string>>();

            Assert.Contains("Cachorro já adotado", result);
        }

        [Fact]
        public async Task GivenAdotarAsync_WhenTutorNaoEncontrado_ThanDeveRetornarErro()
        {
            // Arrange
            var adocaoService = new AdocaoService(
                _uowMock.Object,
                _cachorroRepositoryMock.Object,
                _tutorRepositoryMock.Object);

            var cachorro = _cachorroFixture.CreateManyCachorroWithTutor(1)[0];

            _cachorroRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(cachorro);

            _tutorRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(null as DEPLOY.Cachorro.Domain.Aggregates.Tutor.Entities.Tutor);

            // Act
            var result = await adocaoService.AdotarAsync(Guid.NewGuid(), 1);

            // Assert
            result.Should().BeOfType<List<string>>();

            Assert.Contains("Tutor não encontrado", result);
        }

        [Fact]
        public async Task GivenDevolverAdocaoAsync_WhenCachorroNotFound_ReturnsValidationMessage()
        {
            // Arrange
            var adocaoService = new AdocaoService(
                _uowMock.Object,
                _cachorroRepositoryMock.Object,
                _tutorRepositoryMock.Object);

            var cachorroId = Guid.NewGuid();

            _cachorroRepositoryMock
                .Setup(x => x.GetByIdAsync(cachorroId, default))
                .ReturnsAsync((Domain.Aggregates.Cachorro.Entities.Cachorro)null);

            // Act
            var result = await adocaoService.DevolverAdocaoAsync(cachorroId);

            // Assert
            Assert.Equal("Cachorro não encontrado", result.Single());
            _cachorroRepositoryMock
                .Verify(x => x.UpdateAsync(It.IsAny<Domain.Aggregates.Cachorro.Entities.Cachorro>(), CancellationToken.None), Times.Never);

            _uowMock.Verify(x => x.CommitAndSaveChangesAsync(CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task GivenDevolverAdocaoAsync_WhenCachorroNotAdopted_ReturnsValidationMessage()
        {
            // Arrange
            var adocaoService = new AdocaoService(
                _uowMock.Object,
                _cachorroRepositoryMock.Object,
                _tutorRepositoryMock.Object);

            var cachorro = _cachorroFixture.CreateManyCachorroWithTutor(1)[0];

            _cachorroRepositoryMock
                .Setup(x => x.GetByIdAsync(cachorro.Id, CancellationToken.None))
                .ReturnsAsync(cachorro);

            // Act
            var result = await adocaoService.DevolverAdocaoAsync(cachorro.Id);

            // Assert
            Assert.Equal(Enumerable.Empty<string>(), result);

            _cachorroRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Domain.Aggregates.Cachorro.Entities.Cachorro>(), CancellationToken.None), Times.Once);

            _uowMock.Verify(x => x.CommitAndSaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GivenDevolverAdocaoAsync_WhenCachorroIsAdopted_ReturnsEmptyValidation()
        {
            // Arrange
            var adocaoService = new AdocaoService(
                _uowMock.Object,
                _cachorroRepositoryMock.Object,
                _tutorRepositoryMock.Object);

            var cachorro = _cachorroFixture.CreateManyCachorroWithoutTutor(1)[0];
            cachorro.Erros = new List<string>() { "Cachorro não consta como adotado no sistema" };

            _cachorroRepositoryMock
                .Setup(x => x.GetByIdAsync(cachorro.Id, CancellationToken.None))
                .ReturnsAsync(cachorro);

            // Act
            var result = await adocaoService.DevolverAdocaoAsync(cachorro.Id);

            // Assert
            result.Should().Contain("Cachorro não consta como adotado no sistema");

            _cachorroRepositoryMock.Verify(x => x.UpdateAsync(cachorro, CancellationToken.None), Times.Never);
            _uowMock.Verify(x => x.CommitAndSaveChangesAsync(CancellationToken.None), Times.Never);
        }
    }
}
