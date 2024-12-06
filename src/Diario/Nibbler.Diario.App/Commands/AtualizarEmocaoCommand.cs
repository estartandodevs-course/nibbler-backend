using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.app.Commands;

public class AtualizarEmocaoCommand : Command
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }

    public AtualizarEmocaoCommand(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AtualizarEmocaoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AtualizarEmocaoValidation : AbstractValidator<AtualizarEmocaoCommand>
    {
        public AtualizarEmocaoValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage("Id da emoção inválido.");

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome da emoção é obrigatório.");
        }
    }
}