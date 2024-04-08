using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Repositories;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain.Aggregates.Tutor.Validations
{
    [ExcludeFromCodeCoverage]
    public class TutorValidator : AbstractValidator<Entities.Tutor>
    {
        public readonly ITutorRepository _tutorRepository;

        public TutorValidator(ITutorRepository tutorRepository)
        {
            _tutorRepository = tutorRepository;

            RuleSet("CreateNew", () =>
            {
                RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório");

                RuleFor(x => x.Nome)
                .MustAsync(async (id, cancellation) =>
                {
                    var item = await _tutorRepository.GetByKeyAsync(t => t.Nome == id);

                    if (item.Count == 0)
                        return true;

                    return false;
                })
                .WithMessage("Nome já existe na base");

                RuleFor(x => x.Nome)
               .MaximumLength(100)
               .WithMessage("Nome deve ter no máximo 100 caracteres");

                RuleFor(x => x.CPF)
                .NotEmpty()
                .WithMessage("CPF é obrigatório");

                RuleFor(x => x.CPF.ToString().PadLeft(11,'0'))
                .Must(IsCpf)
                .WithMessage("CPF inválido");

                RuleFor(x => x.CPF.ToString().PadLeft(11, '0'))
                .MustAsync(async (id, cancellation) =>
                {
                    var item = await _tutorRepository.GetByKeyAsync(t => t.CPF == id);

                    if (item.Count == 0)
                        return true;                    

                    return false;
                })
                .WithMessage("CPF já cadastrado");
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório");

                RuleFor(x => x.Nome)
                .MustAsync(async (id, cancellation) =>
                {
                    var item = await _tutorRepository.GetByKeyAsync(t => t.Nome == id);

                    if (item.Count == 0)
                        return true;

                    return false;
                })
                .WithMessage("Está tentando alterar para um Nome que já existe");

                RuleFor(x => x.Nome)
               .MaximumLength(100)
               .WithMessage("Nome deve ter no máximo 100 caracteres");

                RuleFor(x => x.CPF)
                .NotEmpty()
                .WithMessage("CPF é obrigatório");

                RuleFor(x => x.CPF.ToString().PadLeft(11, '0'))
                .Must(IsCpf)
                .WithMessage("CPF inválido");

                RuleFor(x => x.CPF.ToString().PadLeft(11, '0'))
                .MustAsync(async (id, cancellation) =>
                {
                    var item = await _tutorRepository.GetByKeyAsync(t => t.CPF == id);

                    if (item.Count == 0)
                        return true;

                    return false;
                })
                .WithMessage("Está tentando alterar para um CPF que já existe");
            });
        }

        public bool IsCpf(string cpf)
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
