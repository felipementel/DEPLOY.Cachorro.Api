using DEPLOY.Cachorro.Base.Tests;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Validations;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Repositories;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Validations;
using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain.Tests
{
    [ExcludeFromCodeCoverage]
    public class TutorServiceTest : IClassFixture<TutorFixture>
    {
        private readonly TutorFixture _tutorFixture;
        private readonly Mock<ITutorRepository> _tutorRepositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;

        private TutorValidator? _tutorValidator;

        public TutorServiceTest(TutorFixture tutorFixture)
        {
            _tutorFixture = tutorFixture;
            _tutorRepositoryMock = new Mock<ITutorRepository>();
            _uowMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task GivenCreateTutorAsync_WhenTutorIsValid_ThanReturnsCreatedTutor()
        {
            // Arrange
            var expectedTutor = _tutorFixture.CreateManyTutores(1)[0];

            _tutorRepositoryMock
                .Setup(repo => repo.InsertAsync(
                    It.IsAny<Aggregates.Tutor.Entities.Tutor>(),
                    CancellationToken.None))
                .ReturnsAsync(expectedTutor);

            InlineValidator<Domain.Aggregates.Tutor.Entities.Tutor> _validator = new InlineValidator<Aggregates.Tutor.Entities.Tutor>();
            //_validator.RuleFor(t => t.CPF).Must(t => t.Length == 11).WithMessage("CPF inválido");

            ITutorService _tutorService = new TutorService(_uowMock.Object, _validator, _tutorRepositoryMock.Object);

            // Act
            var createdTutor = await _tutorService.CreateAsync(expectedTutor);

            // Assert
            Assert.NotNull(createdTutor);
            Assert.Equal(expectedTutor, createdTutor);

            _tutorRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Aggregates.Tutor.Entities.Tutor>(),
                CancellationToken.None), Times.Once);

            // Assert using FluentAssertions
            createdTutor.Should().NotBeNull();
            createdTutor.Should().BeEquivalentTo(expectedTutor);
        }

        [Fact]
        public async Task GivenCreateTutorAsync_WhenTutorIsNotValid_ThanReturnsCreatedTutor()
        {
            // Arrange
            var expectedTutor = _tutorFixture.CreateManyTutoresWithERRORCPF(1)[0];

            _tutorRepositoryMock.Setup(repo => repo.InsertAsync(
                It.IsAny<Aggregates.Tutor.Entities.Tutor>(),
                CancellationToken.None))
                                   .ReturnsAsync(expectedTutor);
            InlineValidator<Domain.Aggregates.Tutor.Entities.Tutor> _validator = new InlineValidator<Aggregates.Tutor.Entities.Tutor>();
            ITutorService _tutorService = new TutorService(_uowMock.Object, _validator, _tutorRepositoryMock.Object);

            // Act
            var createdTutor = await _tutorService.CreateAsync(expectedTutor);

            // Assert
            Assert.NotNull(createdTutor);
            Assert.Equal(expectedTutor, createdTutor);

            var item = _validator.TestValidate(expectedTutor);
            //item.ShouldHaveValidationErrorFor(t => t.CPF).WithErrorMessage("CPF inválido");

            _tutorRepositoryMock.Verify(repo => repo.InsertAsync(
                    It.IsAny<Aggregates.Tutor.Entities.Tutor>(),
                    CancellationToken.None),
                Times.Once);

            // Assert using FluentAssertions
            createdTutor.Should().NotBeNull();
            createdTutor.Should().BeEquivalentTo(expectedTutor);
        }
    }
}
