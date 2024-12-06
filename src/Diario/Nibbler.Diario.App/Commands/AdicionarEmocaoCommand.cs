using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.app.Commands;

public class AdicionarEmocaoCommand : Command
{
    public string Nome { get; private set; }

    public AdicionarEmocaoCommand(string nome)
    {
        Nome = nome;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AdicionarEmocaoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AdicionarEmocaoValidation : AbstractValidator<AdicionarEmocaoCommand>
    {
        public AdicionarEmocaoValidation()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome da emoção é obrigatório.");
        }
    }
}