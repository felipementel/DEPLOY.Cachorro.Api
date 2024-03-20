using DEPLOY.Cachorro.Base.Tests;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Repositories;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Services;
using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;
using FluentAssertions;
using FluentValidation;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain.Tests
{
    [ExcludeFromCodeCoverage]
    public class CachorroServiceTest : IClassFixture<CachorroFixture>
    {
        private readonly CachorroDtoFixture _cachorroDtoFixture;
        private readonly Mock<ICachorroRepository> _cachorroRepositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private InlineValidator<Domain.Aggregates.Cachorro.Entities.Cachorro> _validator;
        private ICachorroService _cachorroService;

        public CachorroServiceTest()
        {
            _cachorroDtoFixture = new();
            _cachorroRepositoryMock = new Mock<ICachorroRepository>();
            _uowMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task CreateCachorroAsync_ReturnsCreatedCachorro()
        {
            // Arrange
            _validator = new InlineValidator<Domain.Aggregates.Cachorro.Entities.Cachorro>();

            _cachorroService = new CachorroService(_uowMock.Object, _validator, _cachorroRepositoryMock.Object);

            var expectedCachorro = _cachorroDtoFixture.CreateManyCachorroWithTutor(1)[0];

            _cachorroRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Aggregates.Cachorro.Entities.Cachorro>()))
                                   .ReturnsAsync(expectedCachorro);

            // Act
            var createdCachorro = await _cachorroService.CreateAsync(expectedCachorro);

            // Assert
            Assert.NotNull(createdCachorro);
            Assert.Equal(expectedCachorro, createdCachorro);

            _cachorroRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Aggregates.Cachorro.Entities.Cachorro>()), Times.Once);

            // Assert using FluentAssertions
            createdCachorro.Should().NotBeNull();
            createdCachorro.Should().BeEquivalentTo(expectedCachorro);
        }

        [Fact]
        public async Task CreateCachorroAsync_WithInvalidCachorro_ReturnsCreatedCachorro()
        {
            // Arrange
            _validator = new InlineValidator<Domain.Aggregates.Cachorro.Entities.Cachorro>();

            _cachorroService = new CachorroService(_uowMock.Object, _validator, _cachorroRepositoryMock.Object);

            var expectedCachorro = _cachorroDtoFixture.CreateManyCachorroWithTutor(1)[0];

            _cachorroRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Aggregates.Cachorro.Entities.Cachorro>()))
                                   .ReturnsAsync(expectedCachorro);

            // Act
            var createdCachorro = await _cachorroService.CreateAsync(expectedCachorro);

            // Assert
            Assert.NotNull(createdCachorro);
            Assert.Equal(expectedCachorro, createdCachorro);

            _cachorroRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Aggregates.Cachorro.Entities.Cachorro>()), Times.Once);

            // Assert using FluentAssertions
            createdCachorro.Should().NotBeNull();
            createdCachorro.Should().BeEquivalentTo(expectedCachorro);
        }
    }
}
