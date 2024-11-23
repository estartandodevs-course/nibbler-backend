using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.App.Commands;

public class RemoverEntradaCommand : Command
{
    public Guid DiarioId { get; private set; }
    public Guid EntradaId { get; private set; }

    public RemoverEntradaCommand(Guid diarioId, Guid entradaId)
    {
        DiarioId = diarioId;
        EntradaId = entradaId;
    }

    public override bool EstaValido()
    {
        ValidationResult = new RemoverEntradaValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class RemoverEntradaValidation : AbstractValidator<RemoverEntradaCommand>
    {
        public RemoverEntradaValidation()
        {
            RuleFor(c => c.DiarioId)
                .NotEqual(Guid.Empty).WithMessage("Id do diário inválido");

            RuleFor(c => c.EntradaId)
                .NotEqual(Guid.Empty).WithMessage("Id da entrada inválido");
        }
    }
}