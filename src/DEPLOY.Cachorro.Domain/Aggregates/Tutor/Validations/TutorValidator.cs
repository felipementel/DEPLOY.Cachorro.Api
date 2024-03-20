using FluentValidation;

namespace DEPLOY.Cachorro.Domain.Aggregates.Tutor.Validations
{
    public class TutorValidator : AbstractValidator<Entities.Tutor>
    {
        public TutorValidator()
        {
            RuleSet("CreateNew", () =>
            {
                RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório");

                RuleFor(x => x.Nome)
               .MaximumLength(100)
               .WithMessage("Nome deve ter no máximo 100 caracteres");
            });
        }
    }
}
