using Nibbler.Core.Messages;
using FluentValidation;

namespace Nibbler.Diario.App.Commands;

public class AdicionarReflexaoCommand : Command
{
    public Guid DiarioId { get; private set; }
    public string Conteudo { get; private set; }

    public AdicionarReflexaoCommand(Guid diarioId, string conteudo)
    {
        DiarioId = diarioId;
        Conteudo = conteudo;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AdicionarReflexaoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AdicionarReflexaoValidation : AbstractValidator<AdicionarReflexaoCommand>
    {
        public AdicionarReflexaoValidation()
        {
            RuleFor(c => c.DiarioId)
                .NotEqual(Guid.Empty).WithMessage("Id do diário inválido.");

            RuleFor(c => c.Conteudo)
                .NotEmpty().WithMessage("O conteúdo da reflexão é obrigatório.");
        }
    }
}