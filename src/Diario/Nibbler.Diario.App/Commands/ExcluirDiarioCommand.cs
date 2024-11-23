using Nibbler.Core.Messages;
using FluentValidation;

namespace Nibbler.Diario.App.Commands;

public class ExcluirDiarioCommand : Command
{
    public Guid DiarioId { get; private set; }

    public ExcluirDiarioCommand(Guid diarioId)
    {
        DiarioId = diarioId;
    }

    public override bool EstaValido()
    {
        ValidationResult = new ExcluirDiarioValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class ExcluirDiarioValidation : AbstractValidator<ExcluirDiarioCommand>
    {
        public ExcluirDiarioValidation()
        {
            RuleFor(c => c.DiarioId)
                .NotEqual(Guid.Empty).WithMessage("Id do diário inválido.");
        }
    }
}
