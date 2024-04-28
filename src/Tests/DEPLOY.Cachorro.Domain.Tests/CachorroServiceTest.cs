using Bogus;
using DEPLOY.Cachorro.Base.Tests;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Repositories;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Validations;
using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain.Tests
{
    [ExcludeFromCodeCoverage]
    public class CachorroServiceTest : IClassFixture<CachorroFixture>
    {
        private readonly CachorroFixture _cachorroFixture;

        private readonly Mock<ICachorroRepository> _cachorroRepositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;

        private readonly CachorroValidator _cachorroValidator;

        public CachorroServiceTest(CachorroFixture cachorroFixture)
        {
            _cachorroFixture = cachorroFixture;
            _cachorroRepositoryMock = new Mock<ICachorroRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _cachorroValidator = new CachorroValidator(_cachorroRepositoryMock.Object);
        }

        [Fact]
        public async Task GivenCreateCachorroAsync_WhenCachorroIsValid_ThanReturnsCreatedCachorro()
        {
            // Arrange
            InlineValidator<Domain.Aggregates.Cachorro.Entities.Cachorro> _validator = new InlineValidator<Aggregates.Cachorro.Entities.Cachorro>();

            ICachorroService _cachorroService = new CachorroService(
                _uowMock.Object,
                _validator,
                _cachorroRepositoryMock.Object);

            var expectedCachorro = _cachorroFixture
                .CreateManyCachorroWithTutor(1)[0];

            _cachorroRepositoryMock.Setup(repo => repo
            .InsertAsync(
                It.IsAny<Aggregates.Cachorro.Entities.Cachorro>(),
                CancellationToken.None))
            .ReturnsAsync(expectedCachorro);

            // Act
            var createdCachorro = await _cachorroService.CreateAsync(expectedCachorro);

            // Assert
            Assert.NotNull(createdCachorro);
            Assert.Equal(expectedCachorro, createdCachorro);

            _cachorroRepositoryMock.Verify(repo => repo
            .InsertAsync(
                It.IsAny<Aggregates.Cachorro.Entities.Cachorro>(),
                CancellationToken.None)
            , Times.Once);

            // Assert using FluentAssertions
            createdCachorro
                .Should()
                .NotBeNull();

            createdCachorro
                .Should()
                .BeEquivalentTo(expectedCachorro);
        }

        [Fact]
        public async Task GivenCreateCachorroAsync_WhenErroInLine_ThanReturnsError()
        {
            // Arrange
            InlineValidator<Domain.Aggregates.Cachorro.Entities.Cachorro> _validator = new InlineValidator<Aggregates.Cachorro.Entities.Cachorro>();
            _validator.RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório");

            ICachorroService _cachorroService = new CachorroService(
                _uowMock.Object,
                _validator,
                _cachorroRepositoryMock.Object);

            var expectedCachorro = _cachorroFixture
                .CreateManyCachorroWithTutor(1)[0];

            _cachorroRepositoryMock
                .Setup(repo => repo.InsertAsync(
                    It.IsAny<Aggregates.Cachorro.Entities.Cachorro>(),
                    CancellationToken.None))
            .ReturnsAsync(expectedCachorro);

            // Act
            var createdCachorro = await _cachorroService
                .CreateAsync(expectedCachorro);

            // Assert
            Assert.NotNull(createdCachorro);
            Assert.Equal(expectedCachorro, createdCachorro);

            _cachorroRepositoryMock
                .Verify(repo => repo.InsertAsync(
                    It.IsAny<Aggregates.Cachorro.Entities.Cachorro>(),
                    CancellationToken.None), Times.Once);

            // Assert using FluentAssertions
            createdCachorro.Should().NotBeNull();
            createdCachorro.Should().BeEquivalentTo(expectedCachorro);
        }

        [Fact]
        public async Task GivenCreateCachorroAsync_WhenWithInvalidCachorro_ThanReturnsCreatedCachorro()
        {
            // Arrange
            InlineValidator<Domain.Aggregates.Cachorro.Entities.Cachorro> _validator = new InlineValidator<Aggregates.Cachorro.Entities.Cachorro>();

            ICachorroService _cachorroService = new CachorroService(
                _uowMock.Object,
                _validator,
                _cachorroRepositoryMock.Object);

            var expectedCachorro = _cachorroFixture
                .CreateManyCachorroWithNameError(1)[0];

            /*var cachorroValidator = */
            _cachorroValidator
                .TestValidate(expectedCachorro);

            //cachorroValidator
            //    .ShouldHaveValidationErrorFor(c => c.Nome)
            //  .WithErrorMessage("Nome é obrigatório");

            _cachorroRepositoryMock
                .Setup(repo => repo.InsertAsync(
                    It.IsAny<Aggregates.Cachorro.Entities.Cachorro>(),
                    CancellationToken.None))
                .ReturnsAsync(expectedCachorro);

            // Act
            var createdCachorro = await _cachorroService
                .CreateAsync(expectedCachorro);

            // Assert
            Assert.NotNull(createdCachorro);
            Assert.Equal(expectedCachorro, createdCachorro);

            _cachorroRepositoryMock.Verify(repo => repo.InsertAsync(
                    It.IsAny<Aggregates.Cachorro.Entities.Cachorro>(),
                    CancellationToken.None),
                Times.Once);

            // Assert using FluentAssertions
            createdCachorro
                .Should()
                .NotBeNull();

            createdCachorro
                .Should()
                .BeEquivalentTo(expectedCachorro);
        }
    }
}
