using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Validations
{
    [ExcludeFromCodeCoverage]
    public class CachorroValidator : AbstractValidator<Entities.Cachorro>
    {
        public CachorroValidator()
        {
            RuleSet("CreateNew", () =>
            {
                RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório");

                RuleFor(x => x.Nome)
               .MaximumLength(100)
               .WithMessage("Nome deve ter no máximo 100 caracteres");

                RuleFor(x => x.Nascimento)
                .NotEmpty()
                .WithMessage("Data de nascimento é obrigatória");

                RuleFor(x => x.Pelagem)
                .NotEmpty()
                .WithMessage("Pelagem é obrigatória");

                RuleFor(x => x.Peso)
                .NotEmpty()
                .WithMessage("Peso é obrigatório");

                RuleFor(x => x.Peso)
                .GreaterThan(0)
                .WithMessage("Peso deve ser maior que 0");

                RuleFor(x => x.Nascimento.Date)
                .LessThanOrEqualTo(DateTime.Now.Date)
                .WithMessage("Data de nascimento deve ser menor ou igual a data atual");
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id é obrigatório");

                RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório");

                RuleFor(x => x.Nome)
               .MaximumLength(100)
               .WithMessage("Nome deve ter no máximo 100 caracteres");

                RuleFor(x => x.Nascimento)
                .NotEmpty()
                .WithMessage("Data de nascimento é obrigatória");

                RuleFor(x => x.Pelagem)
                .NotEmpty()
                .WithMessage("Pelagem é obrigatória");

                RuleFor(x => x.Peso)
                .NotEmpty()
                .WithMessage("Peso é obrigatório");

                RuleFor(x => x.Peso)
                .GreaterThan(0)
                .WithMessage("Peso deve ser maior que 0");
            });
        }
    }
}
