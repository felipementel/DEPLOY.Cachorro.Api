using DEPLOY.Cachorro.Base.Tests;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Repositories;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Validations;
using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain.Tests
{
    [ExcludeFromCodeCoverage]
    public class TutorServiceTest : IClassFixture<TutorFixture>
    {
        private readonly TutorFixture _tutorFixture;
        private readonly Mock<ITutorRepository> _tutorRepositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;

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

            InlineValidator<Domain.Aggregates.Tutor.Entities.Tutor> _tutorValidator = new InlineValidator<Aggregates.Tutor.Entities.Tutor>();
            //_validator.RuleFor(t => t.CPF).Must(t => t.Length == 11).WithMessage("CPF inválido");

            ITutorService _tutorService = new TutorService(_uowMock.Object, _tutorValidator, _tutorRepositoryMock.Object);

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
            InlineValidator<Domain.Aggregates.Tutor.Entities.Tutor> _tutorValidator = new InlineValidator<Aggregates.Tutor.Entities.Tutor>();
            _tutorValidator.RuleFor(t => t.CPF).Must(IsCpf).WithMessage("CPF inválido");

            ITutorService _tutorService = new TutorService(_uowMock.Object, _tutorValidator, _tutorRepositoryMock.Object);

            // Act
            //var result = _tutorValidator.TestValidate(expectedTutor);

            var item = _tutorValidator.TestValidate(expectedTutor);
            var createdTutor = await _tutorService.CreateAsync(expectedTutor);            

            // Assert
            Assert.NotNull(createdTutor);
            Assert.Equal(expectedTutor, createdTutor);

            item.ShouldHaveValidationErrorFor(t => t.CPF).WithErrorMessage("CPF inválido");


            _tutorRepositoryMock.Verify(repo => repo.InsertAsync(
                    It.IsAny<Aggregates.Tutor.Entities.Tutor>(),
                    CancellationToken.None),
                Times.Once);

            // Assert using FluentAssertions
            createdTutor.Should().NotBeNull();
            createdTutor.Should().BeEquivalentTo(expectedTutor);
        }

        private bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}
